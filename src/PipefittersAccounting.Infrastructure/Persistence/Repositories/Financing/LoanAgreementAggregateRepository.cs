#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604

using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing
{
    public class LoanAgreementAggregateRepository : ILoanAgreementAggregateRepository
    {
        private bool _isDisposed;
        private readonly AppDbContext _dbContext;

        public LoanAgreementAggregateRepository(AppDbContext ctx) => _dbContext = ctx;

        ~LoanAgreementAggregateRepository() => Dispose(false);

        public async Task<OperationResult<LoanAgreement>> GetByIdAsync(Guid id)
        {
            try
            {
                LoanAgreement? agreement = await _dbContext.LoanAgreements.Where(la => la.Id == id)
                                                                          .Include(i => i.LoanAmortizationTable.Where(a => a.LoanId == id))
                                                                          .FirstOrDefaultAsync();

                if (agreement is not null)
                {
                    return OperationResult<LoanAgreement>.CreateSuccessResult(agreement);
                }
                else
                {
                    string errMsg = $"Unable to find a loan agreement with LoanId: {id}.";
                    return OperationResult<LoanAgreement>.CreateFailure(errMsg);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<LoanAgreement>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> Exists(Guid id)
        {
            try
            {
                LoanAgreement? agreement = await _dbContext.LoanAgreements.FindAsync(id);

                if (agreement is not null)
                {
                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    return OperationResult<bool>.CreateSuccessResult(false);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddAsync(LoanAgreement entity)
        {
            try
            {
                EconomicEvent evt = new(EntityGuidID.Create(entity.Id), EventTypeEnum.LoanAgreement);
                await _dbContext.EconomicEvents.AddAsync(evt);
                await _dbContext.LoanAgreements.AddAsync(entity);

                foreach (LoanInstallment installment in entity.LoanAmortizationTable)
                {
                    await _dbContext.EconomicEvents.AddAsync(new(EntityGuidID.Create(installment.Id),
                                                                                     EventTypeEnum.LoanInstallment));
                    await _dbContext.LoanInstallments.AddAsync(installment);
                }

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }

        }

        public OperationResult<bool> Update(LoanAgreement entity) => throw new NotImplementedException();

        public OperationResult<bool> Delete(LoanAgreement entity)
        {
            try
            {
                entity.LoanAmortizationTable.ToList().ForEach(i => _dbContext.LoanInstallments.Remove(i));
                entity.LoanAmortizationTable.ToList()
                                            .ForEach(i =>
                                            {
                                                EconomicEvent evt = _dbContext.EconomicEvents.Find(i.Id);
                                                _dbContext.EconomicEvents.Remove(evt);
                                            });

                _dbContext.LoanAgreements.Remove(entity);
                EconomicEvent? evt = _dbContext.EconomicEvents.Find(entity.Id);
                _dbContext.EconomicEvents.Remove(evt);

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
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