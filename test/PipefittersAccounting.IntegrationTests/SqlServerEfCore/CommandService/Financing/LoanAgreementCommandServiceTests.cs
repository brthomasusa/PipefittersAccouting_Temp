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
    public class LoanAgreementCommandServiceTests : TestBaseEfCore
    {
        [Fact]
        public async Task Create_LoanAgreement_WithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);
            ILoanAgreementCommandService cmdSvc = new LoanAgreementCommandService(repo, uow);

            CreateLoanAgreementInfo model = LoanAgreementTestData.GetCreateLoanAgreementInfo();

            OperationResult<bool> result = await cmdSvc.CreateLoanAgreement(model);
            Assert.True(result.Success);

            var agreement = await repo.GetByIdAsync(model.LoanId);

            Assert.NotNull(agreement);
        }

        [Fact]
        public async Task Delete_LoanAgreement_WithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            ILoanAgreementAggregateRepository repo = new LoanAgreementAggregateRepository(_dbContext);
            ILoanAgreementCommandService cmdSvc = new LoanAgreementCommandService(repo, uow);

            DeleteLoanAgreementInfo model = new DeleteLoanAgreementInfo
            {
                LoanId = new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867"),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };


            OperationResult<bool> result = await cmdSvc.DeleteLoanAgreement(model);
            Assert.True(result.Result);

            OperationResult<bool> exist = await repo.Exists(model.LoanId);

            Assert.False(exist.Result);
        }
    }
}