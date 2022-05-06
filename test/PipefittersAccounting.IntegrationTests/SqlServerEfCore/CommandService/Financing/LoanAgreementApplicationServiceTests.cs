#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Core.Interfaces.Financing;
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
        private readonly AppUnitOfWork _unitOfWork;
        private readonly ILoanAgreementAggregateRepository _repository;
        private readonly ILoanAgreementApplicationService _cmdService;
        public LoanAgreementApplicationServiceTests()
        {
            _unitOfWork = new AppUnitOfWork(_dbContext);
            _repository = new LoanAgreementAggregateRepository(_dbContext);
            _cmdService = new LoanAgreementApplicationService(_repository, _unitOfWork);
        }

        [Fact]
        public async Task Create_LoanAgreement_WithValidInfo_ShouldSucceed()
        {
            CreateLoanAgreementInfo model = LoanAgreementTestData.GetCreateLoanAgreementInfo();

            OperationResult<bool> result = await _cmdService.CreateLoanAgreement(model);
            Assert.True(result.Success);

            var agreement = await _repository.GetByIdAsync(model.LoanId);

            Assert.NotNull(agreement);
        }

        [Fact]
        public async Task Delete_LoanAgreement_WithValidInfo_ShouldSucceed()
        {
            DeleteLoanAgreementInfo model = new DeleteLoanAgreementInfo
            {
                LoanId = new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867"),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

            OperationResult<bool> result = await _cmdService.DeleteLoanAgreement(model);
            Assert.True(result.Result);

            OperationResult<bool> exist = await _repository.Exists(model.LoanId);

            Assert.False(exist.Result);
        }
    }
}