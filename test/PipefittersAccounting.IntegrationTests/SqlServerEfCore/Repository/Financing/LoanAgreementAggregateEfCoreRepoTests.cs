#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
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
        public async Task Exists_LoanAgreementAggregateRepository_ReturnTrue()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            Guid loanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");

            OperationResult<bool> result = await repo.Exists(loanId);
            Assert.True(result.Success);

            LoanAgreement agreement = await _dbContext.LoanAgreements.FindAsync(loanId);
            Assert.NotNull(agreement);
        }

        [Fact]
        public async Task GetById_LoanAgreementAggregateRepository_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            Guid loanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");

            OperationResult<LoanAgreement> result = await repo.GetByIdAsync(loanId);
            Assert.True(result.Success);

            int installmentCount = result.Result.LoanAmortizationTable.Count;
            Assert.Equal(12, installmentCount);
            Assert.Equal(new Guid("12998229-7ede-4834-825a-0c55bde75695"), result.Result.FinancierId);
        }

        [Fact]
        public async Task AddAsync_NewStandardLoanAgreementWithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            LoanAgreement agreement = LoanAgreementTestData.GetStandardLoanAgreementForCreating();

            OperationResult<bool> addResult = await repo.AddAsync(agreement);

            Assert.True(addResult.Success);
            await uow.Commit();

            OperationResult<LoanAgreement> result = await repo.GetByIdAsync(agreement.Id);
            Assert.True(result.Success);
            Assert.Equal(agreement.FinancierId, result.Result.FinancierId);
        }

        [Fact]
        public async Task AddAsync_NewCustomLoanAgreementWithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            LoanAgreement agreement = LoanAgreementTestData.GetCustomLoanAgreementForCreating();

            OperationResult<bool> addResult = await repo.AddAsync(agreement);

            Assert.True(addResult.Success);
            await uow.Commit();

            OperationResult<LoanAgreement> result = await repo.GetByIdAsync(agreement.Id);
            Assert.True(result.Success);
            Assert.Equal(agreement.FinancierId, result.Result.FinancierId);
        }

        // [Fact]
        // public async Task Update_EditLoanAgreementInfo_ShouldSucceed()
        // {
        //     AppUnitOfWork uow = new(_dbContext);
        //     ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

        //     LoanAgreement agreement = TestUtilities.GetLoanAgreementForEditing();
        //     agreement.UpdateLoanAmount(LoanAmount.Create(44000M));
        //     agreement.UpdateInterestRate(InterestRate.Create(.0515M));
        //     agreement.UpdateMaturityDate(MaturityDate.Create(new DateTime(2023, 6, 15)));

        //     repo.Update(agreement);

        //     var result = await _dbContext.LoanAgreements.FindAsync(agreement.Id);
        //     Assert.Equal(44000M, result.LoanAmount);
        //     Assert.Equal(.0515M, result.InterestRate);
        //     Assert.Equal(new DateTime(2023, 6, 15), result.MaturityDate);
        // }

        [Fact]
        public async Task Delete_DeleteLoanAgreementInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);

            Guid loanId = new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867");

            LoanAgreement agreement = await _dbContext.LoanAgreements.FindAsync(loanId);
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