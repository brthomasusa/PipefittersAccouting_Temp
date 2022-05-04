#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    public class CashAccountAggregateCommandServiceTests : TestBaseEfCore
    {
        private readonly ICashAccountApplicationService _appService;
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private readonly AppUnitOfWork _unitOfWork;

        public CashAccountAggregateCommandServiceTests()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            _repository = new CashAccountAggregateRepository(_dbContext);
            _validationService = new CashAccountAggregateValidationService(queryService);
            _unitOfWork = new AppUnitOfWork(_dbContext);
            _appService = new CashAccountApplicationService(_validationService, _repository, _unitOfWork);
        }

        [Fact]
        public async void Process_CashAccountCreateCommand_ShouldSucceed()
        {
            CashAccountCreateCommand cmd = new(_validationService);
            CreateCashAccountInfo model = GetCreateCashAccountInfo();

            OperationResult<bool> result = await cmd.Process(model, _repository, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CashAccountUpdateCommand_ShouldSucceed()
        {
            CashAccountUpdateCommand cmd = new(_validationService);
            EditCashAccountInfo model = GetEditCashAccountInfo();

            OperationResult<bool> result = await cmd.Process(model, _repository, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CashAccountDeleteCommand_ShouldSucceed()
        {
            CashAccountDeleteCommand cmd = new(_validationService);
            DeleteCashAccountInfo model
                = new()
                {
                    CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                };

            OperationResult<bool> result = await cmd.Process(model, _repository, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void CreateCashAccount_CashAccountApplicationService_ShouldSucceed()
        {
            CreateCashAccountInfo model = GetCreateCashAccountInfo();

            OperationResult<bool> result = await _appService.CreateCashAccount(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async void UpdateCashAccount_CashAccountApplicationService_ShouldSucceed()
        {
            EditCashAccountInfo model = GetEditCashAccountInfo();

            OperationResult<bool> result = await _appService.UpdateCashAccount(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async void DeleteCashAccount_CashAccountApplicationService_ShouldSucceed()
        {
            DeleteCashAccountInfo model
                = new()
                {
                    CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                };

            OperationResult<bool> result = await _appService.DeleteCashAccount(model);

            Assert.True(result.Success);
        }

        private CreateCashAccountInfo GetCreateCashAccountInfo()
            => new CreateCashAccountInfo
            {
                CashAccountId = new Guid("210d34d7-7474-44e7-a90b-93998137917a"),
                CashAccountType = (int)CashAccountTypeEnum.NonPayrollOperations,
                BankName = "Big Bank",
                CashAccountName = "Party Party Party!",
                CashAccountNumber = "123456789",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 3),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        private EditCashAccountInfo GetEditCashAccountInfo()
            => new EditCashAccountInfo
            {
                CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                CashAccountType = (int)CashAccountTypeEnum.PayrollOperations,
                BankName = "Big Bank",
                CashAccountName = "Party Party Party!",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 3),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
    }
}
