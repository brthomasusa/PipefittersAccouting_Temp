#pragma warning disable CS8600

using Microsoft.EntityFrameworkCore;

using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing
{
    public class StockSubscriptionAggregateRepository : IStockSubscriptionAggregateRepository
    {
        private bool _isDisposed;
        private readonly AppDbContext _dbContext;

        public StockSubscriptionAggregateRepository(AppDbContext ctx) => _dbContext = ctx;
        ~StockSubscriptionAggregateRepository() => Dispose(false);

        public async Task<OperationResult<StockSubscription>> GetStockSubscriptionByIdAsync(Guid stockId)
        {
            try
            {
                StockSubscription? subscription = await _dbContext.StockSubscriptions.Where(subs => subs.Id == stockId)
                                                                                     .Include(d => d.DividendDeclarations.Where(a => a.StockId == stockId))
                                                                                     .FirstOrDefaultAsync();

                if (subscription is null)
                {
                    string msg = $"Unable to locate a stock subscription with StockId '{stockId}'.";
                    return OperationResult<StockSubscription>.CreateFailure(msg);
                }

                return OperationResult<StockSubscription>.CreateSuccessResult(subscription);
            }
            catch (Exception ex)
            {
                return OperationResult<StockSubscription>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<DividendDeclaration>> GetDividendDeclarationByIdAsync(Guid dividendId)
        {
            try
            {
                DividendDeclaration? dividend = await _dbContext.DividendDeclarations.Where(d => d.Id == dividendId)
                                                                                     .FirstOrDefaultAsync();

                if (dividend is null)
                {
                    string msg = $"Unable to locate a dividend declaration with DividendId '{dividendId}'.";
                    return OperationResult<DividendDeclaration>.CreateFailure(msg);
                }

                return OperationResult<DividendDeclaration>.CreateSuccessResult(dividend);
            }
            catch (Exception ex)
            {
                return OperationResult<DividendDeclaration>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> DoesStockSubscriptionExist(Guid stockId)
        {
            try
            {
                bool exist = await _dbContext.StockSubscriptions.FindAsync(stockId) != null;

                if (exist)
                {
                    return OperationResult<bool>.CreateSuccessResult(exist);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure($"A stock subscription with StockId '{stockId}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddStockSubscriptionAsync(StockSubscription subscription)
        {
            try
            {
                OperationResult<bool> searchResult = await DoesStockSubscriptionExist(subscription.Id);

                if (searchResult.Result)
                {
                    string errMsg = $"A stock subscription with StockId '{subscription.Id}' is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeSubscriptionResult = await CheckForDuplicateStockSubscription(subscription.FinancierId,
                                                                                                        subscription.SharesIssured,
                                                                                                        subscription.PricePerShare,
                                                                                                        subscription.StockIssueDate);

                if (dupeSubscriptionResult.Result != Guid.Empty)
                {
                    string errMsg = $"A stock subscription (StockId: {dupeSubscriptionResult.Result}) with the same FinancierId, SharesIssued, PricePerShare, and StockIssueDate is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                EconomicEvent economicEvent = new(EntityGuidID.Create(subscription.Id), EventTypeEnum.StockSubscription);
                await _dbContext.EconomicEvents.AddAsync(economicEvent);
                await _dbContext.StockSubscriptions.AddAsync(subscription);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> UpdateStockSubscriptionAsync(StockSubscription subscription)
        {
            try
            {
                OperationResult<bool> searchResult = await DoesStockSubscriptionExist(subscription.Id);

                if (!searchResult.Result)
                {
                    string errMsg = $"Update failed! A stock subscription with StockId '{subscription.Id}' could not be located.";
                    return OperationResult<bool>.CreateFailure(errMsg); ;
                }

                OperationResult<Guid> dupeSubscriptionResult = await CheckForDuplicateStockSubscription(subscription.FinancierId,
                                                                                                        subscription.SharesIssured,
                                                                                                        subscription.PricePerShare,
                                                                                                        subscription.StockIssueDate);

                if (dupeSubscriptionResult.Result != Guid.Empty && dupeSubscriptionResult.Result != subscription.Id)
                {
                    string errMsg = $"Update failed! Your edits would result in a duplicate stock subscription that matches stock subscription with Id '{dupeSubscriptionResult.Result}'.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                _dbContext.StockSubscriptions.Update(subscription);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> DeleteStockSubscriptionAsync(Guid stockId)
        {
            try
            {
                StockSubscription subscription = await _dbContext.StockSubscriptions.FindAsync(stockId);

                if (subscription is not null)
                {
                    EconomicEvent economicEvent = await _dbContext.EconomicEvents.FindAsync(stockId);
                    if (economicEvent is not null)
                    {
                        _dbContext.StockSubscriptions.Remove(subscription);
                        _dbContext.EconomicEvents.Remove(economicEvent);
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        string msg = $"Delete stock subscription failed! Unable to locate EconomicEvent associated with this subscription.";
                        return OperationResult<bool>.CreateFailure(msg);
                    }

                }
                else
                {
                    string errMsg = $"Delete failed! A stock subscription with StockId: {stockId} could not be located.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        private async Task<OperationResult<Guid>> CheckForDuplicateStockSubscription
        (
            Guid financierId,
            int sharesIssured,
            decimal pricePerShare,
            DateTime stockIssueDate
        )
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from subscription in _dbContext.StockSubscriptions.Where(e => e.FinancierId == financierId &&
                                                                                                   e.SharesIssured == sharesIssured &&
                                                                                                   e.PricePerShare == pricePerShare &&
                                                                                                   e.StockIssueDate == stockIssueDate)
                                                                                       .AsNoTracking()
                                     select new { StockId = subscription.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.StockId;
                }

                return OperationResult<Guid>.CreateSuccessResult(returnValue);
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex);
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // free managed resources
                _dbContext.Dispose();
            }

            _isDisposed = true;
        }
    }
}