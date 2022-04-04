#pragma warning disable CS8602
#pragma warning disable CS8603

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
        private bool isDisposed;
        private readonly AppDbContext _dbContext;

        public LoanAgreementAggregateRepository(AppDbContext ctx) => _dbContext = ctx;

        ~LoanAgreementAggregateRepository() => Dispose(false);

        public async Task<LoanAgreement> GetByIdAsync(Guid id) => await _dbContext.LoanAgreements.FindAsync(id);

        public async Task<bool> Exists(Guid id) => await _dbContext.LoanAgreements.FindAsync(id) != null;

        public async Task AddAsync(LoanAgreement entity)
        {
            EconomicEvent evt = new(EntityGuidID.Create(entity.Id), EventTypeEnum.LoanAgreementCashReceipt);
            await _dbContext.EconomicEvents.AddAsync(evt);
            await _dbContext.LoanAgreements.AddAsync(entity);
        }

        public void Update(LoanAgreement entity) => _dbContext.LoanAgreements.Update(entity);

        public void Delete(LoanAgreement entity)
        {
            _dbContext.LoanAgreements.Remove(entity);

            string errMsg = $"Delete loan agreement failed, unable to locate economic event with id: {entity.Id}";
            EconomicEvent evt = _dbContext.EconomicEvents.Find(entity.Id) ?? throw new ArgumentNullException(errMsg);

            _dbContext.EconomicEvents.Remove(evt);
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