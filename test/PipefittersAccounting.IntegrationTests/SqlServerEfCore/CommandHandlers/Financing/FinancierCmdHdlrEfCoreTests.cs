#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
// using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
// using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandHandlers.Financing
{
    [Trait("Integration", "EfCoreCmdHdlr")]
    public class FinancierCmdHdlrEfCoreTests
    {

    }
}