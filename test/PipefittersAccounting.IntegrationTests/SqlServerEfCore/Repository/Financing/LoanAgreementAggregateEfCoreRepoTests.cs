#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.Repository.Financing
{
    public class LoanAgreementAggregateEfCoreRepoTests : TestBaseEfCore
    {
        [Fact]
        public async Task Exists_DoesLoanAgreementExist_ReturnTrue()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            Guid loanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");

            bool result = await repo.Exists(loanId);
            Assert.True(result);

            LoanAgreement agreement = await _dbContext.LoanAgreements.FindAsync(loanId);
            Assert.NotNull(agreement);
        }

        [Fact]
        public async Task AddAsync_NewLoanAgreementWithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            LoanAgreement agreement = TestUtilities.GetLoanAgreementForCreating();

            await repo.AddAsync(agreement);
            await uow.Commit();

            var result = await repo.GetByIdAsync(agreement.Id);
            Assert.NotNull(result);
            Assert.Equal(agreement.FinancierId, result.FinancierId);
        }

        [Fact]
        public async Task Update_EditLoanAgreementInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            LoanAgreement agreement = TestUtilities.GetLoanAgreementForEditing();
            agreement.UpdateLoanAmount(LoanAmount.Create(44000M));
            agreement.UpdateInterestRate(InterestRate.Create(.0515));
            agreement.UpdateMaturityDate(MaturityDate.Create(new DateTime(2023, 6, 15)));

            repo.Update(agreement);

            var result = await _dbContext.LoanAgreements.FindAsync(agreement.Id);
            Assert.Equal(44000M, result.LoanAmount);
            Assert.Equal(.0515, result.InterestRate);
            Assert.Equal(new DateTime(2023, 6, 15), result.MaturityDate);
        }

        [Fact]
        public async Task Delete_DeleteLoanAgreementInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            LoanAgreement agreement = await _dbContext.LoanAgreements.FindAsync(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"));
            EconomicEvent evt = await _dbContext.EconomicEvents.FindAsync(agreement.Id);
            Assert.NotNull(agreement);
            Assert.NotNull(evt);

            repo.Delete(agreement);
            await uow.Commit();

            LoanAgreement ageementResult = await _dbContext.LoanAgreements.FindAsync(agreement.Id);
            EconomicEvent eventResult = await _dbContext.EconomicEvents.FindAsync(agreement.Id);

            Assert.Null(ageementResult);
            Assert.Null(eventResult);
        }
    }
}