#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
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
    public class LoanAgreementWriteCommandsTests : TestBaseEfCore
    {
        private readonly ILoanAgreementAggregateRepository _repository;
        private readonly ILoanAgreementValidationService _validationService;
        private readonly IUnitOfWork _unitOfWork;

        public LoanAgreementWriteCommandsTests()
        {
            ILoanAgreementQueryService loanAgreementQrySvc = new LoanAgreementQueryService(_dapperCtx);
            ISharedQueryService sharedQueryService = new SharedQueryService(_dapperCtx);
            IQueryServicesRegistry servicesRegistry = new QueryServicesRegistry();

            _validationService = new LoanAgreementValidationService(loanAgreementQrySvc, sharedQueryService, servicesRegistry);
            _repository = new LoanAgreementAggregateRepository(_dbContext);
            _unitOfWork = new AppUnitOfWork(_dbContext);
        }

        [Fact]
        public void Instantiate_LoanAgreementCreateCommand_FromBaseClass_ShouldSucceed()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();

            WriteCommandHandler<LoanAgreementWriteModel, ILoanAgreementAggregateRepository, ILoanAgreementValidationService, LoanAgreement> createCommand
                = new LoanAgreementCreateCommandv2(model, _repository, _validationService, _unitOfWork);

            Assert.IsType<LoanAgreementCreateCommandv2>(createCommand);
        }

        [Fact]
        public void Instantiate_LoanAgreementCreateCommand_FromDerivedClass_ShouldSucceed()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            LoanAgreementCreateCommandv2 createCommand = new(model, _repository, _validationService, _unitOfWork);

            Assert.IsType<LoanAgreementCreateCommandv2>(createCommand);
        }

        [Fact]
        public async Task Process_LoanAgreementCreateCommand_WithValidInfo_ShouldSucceed()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            LoanAgreementCreateCommandv2 createCommand = new(model, _repository, _validationService, _unitOfWork);

            OperationResult<bool> result = await createCommand.Process();

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_LoanAgreementCreateCommand_InvalidCreditor_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            LoanAgreementCreateCommandv2 createCommand = new(model, _repository, _validationService, _unitOfWork);
            OperationResult<bool> result = await createCommand.Process();

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_LoanAgreementCreateCommand_Duplicate_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            model.LoanAmount = 30000M;
            model.InterestRate = 0.0863M;
            model.LoanDate = new DateTime(2022, 2, 2);
            model.MaturityDate = new DateTime(2024, 2, 2);

            LoanAgreementCreateCommandv2 createCommand = new(model, _repository, _validationService, _unitOfWork);
            OperationResult<bool> result = await createCommand.Process();

            Assert.False(result.Success);
        }



































    }
}
