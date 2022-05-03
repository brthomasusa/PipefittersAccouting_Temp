#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603

using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing
{
    public class FinancierAggregateRepository : IFinancierAggregateRepository, IDisposable
    {
        private bool isDisposed;
        private readonly AppDbContext _dbContext;

        public FinancierAggregateRepository(AppDbContext ctx) => _dbContext = ctx;

        ~FinancierAggregateRepository() => Dispose(false);

        public async Task<OperationResult<Financier>> GetByIdAsync(Guid id)
        {
            try
            {
                Financier financier = await _dbContext.Financiers.FindAsync(id);

                if (financier is null)
                {
                    string msg = $"Unable to locate financier with id '{id}'.";
                    return OperationResult<Financier>.CreateFailure(msg);
                }

                return OperationResult<Financier>.CreateSuccessResult(financier);
            }
            catch (Exception ex)
            {
                return OperationResult<Financier>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> Exists(Guid id)
        {
            try
            {
                bool exist = await _dbContext.Financiers.FindAsync(id) != null;
                if (exist)
                {
                    return OperationResult<bool>.CreateSuccessResult(exist);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure($"A financier with id '{id}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddAsync(Financier entity)
        {
            try
            {
                ExternalAgent agent = new(EntityGuidID.Create(entity.Id), AgentTypeEnum.Financier);
                await _dbContext.ExternalAgents.AddAsync(agent);
                await _dbContext.Financiers.AddAsync(entity);

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> Update(Financier entity)
        {
            try
            {
                _dbContext.Financiers.Update(entity);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> Delete(Financier entity)
        {
            try
            {
                _dbContext.Financiers.Remove(entity);

                string errMsg = $"Delete financier failed, unable to locate external agent with id: {entity.Id}";
                ExternalAgent agent = _dbContext.ExternalAgents.Find(entity.Id) ?? throw new ArgumentNullException(errMsg);

                _dbContext.ExternalAgents.Remove(agent);

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateFinancierName(string name)
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from fin in _dbContext.Financiers.Where(e => String.Equals(e.FinancierName, name))
                                                                      .AsNoTracking()
                                     select new { FinancierId = fin.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.FinancierId;
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
            if (isDisposed) return;

            if (disposing)
            {
                // free managed resources
                _dbContext.Dispose();
            }

            isDisposed = true;
        }
    }
}