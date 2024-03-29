#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class CashAccountAggregateTests
    {
        public CashAccountAggregateTests() { }

        [Fact]
        public void Create_CashAccount_ShouldSucceed()
        {
            var mock = new Mock<ICashAccountAggregateValidationService>();
            ICashAccountAggregateValidationService validationService = mock.Object;

            var exception = Record.Exception(() =>
                                CashAccountTestData.GetCashAccount()
                            );

            Assert.Null(exception);
        }
    }
}