#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.Repository.Financing
{
    [Trait("Integration", "EfCoreRepo")]
    public class FinancierAggregateEfCoreRepoTests : TestBaseEfCore
    {
        [Fact]
        public async Task AddAsync_NewFinancierWithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

            Financier financier = FinancierTestData.GetFinancierForCreating();
            Guid agentId = financier.Id;

            await repo.AddAsync(financier);
            await uow.Commit();

            var result = await repo.GetByIdAsync(financier.Id);
            Assert.NotNull(result);
            Assert.Equal(financier.FinancierName, result.Result.FinancierName);
        }

        [Fact]
        public async Task Exists_DoesFinancierExist_ReturnTrue()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

            Guid agentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            OperationResult<bool> result = await repo.Exists(agentId);
            Assert.True(result.Success);

            Financier financier = await _dbContext.Financiers.FindAsync(agentId);
            Assert.NotNull(financier);
        }

        [Fact]
        public async Task Exists_DoesFinancierExist_ReturnFalse()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

            Guid agentId = new Guid("0000cf34-1234-4fb2-b70e-ab19d3371886");

            OperationResult<bool> result = await repo.Exists(agentId);
            Assert.False(result.Success);

            Financier financier = await _dbContext.Financiers.FindAsync(agentId);
            Assert.Null(financier);
        }

        // [Fact]
        // public async Task Update_EditFinancierInfo_ShouldSucceed()
        // {
        //     IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

        //     Guid agentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");
        //     OperationResult<Financier> financier = await repo.GetByIdAsync(agentId);

        //     Assert.Equal("New World Tatoo Parlor", financier.Result.FinancierName);
        //     Assert.Equal("630-321-9875", financier.Result.FinancierTelephone);
        //     Assert.Equal("1690 S. El Camino Real", financier.Result.FinancierAddress.AddressLine1);
        //     Assert.Equal("Jozef Jr.", financier.Result.PointOfContact.LastName);

        //     financier.Result.UpdateFinancierName(OrganizationName.Create("Money Laundering, Inc."));
        //     financier.Result.UpdateFinancierTelephone(PhoneNumber.Create("630-321-0000"));
        //     financier.Result.UpdateFinancierAddress(Address.Create("123 Main", "2T", "Jacksonville", "MS", "39023"));
        //     financier.Result.UpdatePointOfContact(PointOfContact.Create("Slim", "Shady", "S", "555-211-9874"));

        //     repo.Update(financier.Result);

        //     Financier result = await repo.GetByIdAsync(agentId);

        //     Assert.Equal("Money Laundering, Inc.", result.FinancierName);
        //     Assert.Equal("630-321-0000", result.FinancierTelephone);
        //     Assert.Equal("123 Main", result.FinancierAddress.AddressLine1);
        //     Assert.Equal("Shady", result.PointOfContact.LastName);
        // }

        // [Fact]
        // public async Task Delete_DeleteFinancierInfo_ShouldSucceed()
        // {
        //     AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
        //     IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

        //     Guid agentId = new Guid("84164388-28ff-4b47-bd63-dd9326d32236");
        //     Financier financier = await repo.GetByIdAsync(agentId);

        //     Assert.NotNull(financier);

        //     repo.Delete(financier);
        //     await uow.Commit();

        //     bool result = await repo.Exists(financier.Id);
        //     Assert.False(result);
        // }

        // [Fact]
        // public async Task CheckForDuplicateFinancierName_NameExists_ShouldReturnNonEmptyGuid()
        // {
        //     IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

        //     OperationResult<Guid> result = await repo.CheckForDuplicateFinancierName("Paul Van Horn Enterprises");

        //     Assert.True(result.Result != Guid.Empty);
        // }

        // [Fact]
        // public async Task CheckForDuplicateFinancierName_CaseInsensitive_ShouldReturnNonEmptyGuid()
        // {
        //     IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

        //     OperationResult<Guid> result = await repo.CheckForDuplicateFinancierName("PAUL Van Horn Enterprises");

        //     Assert.True(result.Result != Guid.Empty);
        // }

        // [Fact]
        // public async Task CheckForDuplicateFinancierName_NameDoesNotExists_ShouldReturnEmptyGuid()
        // {
        //     IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);

        //     OperationResult<Guid> result = await repo.CheckForDuplicateFinancierName("Hello C# World");

        //     Assert.True(result.Result == Guid.Empty);
        // }
    }
}