#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    [Trait("Integration", "EfCoreCmdSvc")]
    public class LoanAgreementApplicationServiceTests : TestBaseEfCore
    {
        private readonly ILoanAgreementApplicationService _cmdService;

        public LoanAgreementApplicationServiceTests()
        {
            ILoanAgreementQueryService loanAgreementQrySvc = new LoanAgreementQueryService(_dapperCtx);
            ISharedQueryService sharedQueryService = new SharedQueryService(_dapperCtx);
            IQueryServicesRegistry servicesRegistry = new QueryServicesRegistry();
            ILoanAgreementValidationService validationService =
                new LoanAgreementValidationService(loanAgreementQrySvc, sharedQueryService, servicesRegistry);

            AppUnitOfWork unitOfWork = new AppUnitOfWork(_dbContext);
            ILoanAgreementAggregateRepository repository = new LoanAgreementAggregateRepository(_dbContext);
            _cmdService = new LoanAgreementApplicationService(validationService, repository, unitOfWork);
        }

        [Fact]
        public async Task CreateLoanAgreement_LoanAgreementApplicationService_ShouldReturnTrue()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();

            OperationResult<bool> result = await _cmdService.CreateLoanAgreement(model);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateLoanAgreement_LoanAgreementApplicationService_InvalidCreditor_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<bool> result = await _cmdService.CreateLoanAgreement(model);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateLoanAgreement_LoanAgreementApplicationService_Duplicate_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            model.LoanAmount = 30000M;
            model.InterestRate = 0.0863M;
            model.LoanDate = new DateTime(2022, 2, 2);
            model.MaturityDate = new DateTime(2024, 2, 2);

            OperationResult<bool> result = await _cmdService.CreateLoanAgreement(model);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteLoanAgreement_LoanAgreementApplicationService_ShouldReturnTrue()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();

            OperationResult<bool> result = await _cmdService.DeleteLoanAgreement(model);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task DeleteLoanAgreement_LoanAgreementApplicationService_InvalidCreditor_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            model.FinancierId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<bool> result = await _cmdService.DeleteLoanAgreement(model);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteLoanAgreement_LoanAgreementApplicationService_InvalidLoanAgreement_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            model.LoanId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<bool> result = await _cmdService.DeleteLoanAgreement(model);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteLoanAgreement_LoanAgreementApplicationService_CreditorNotLinkedToLoanAgreement_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            model.LoanId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            OperationResult<bool> result = await _cmdService.DeleteLoanAgreement(model);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteLoanAgreement_LoanAgreementApplicationService_LoanProceedsHaveBeenDeposited_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithDeposit();

            OperationResult<bool> result = await _cmdService.DeleteLoanAgreement(model);
            Assert.False(result.Success);
        }
    }
}