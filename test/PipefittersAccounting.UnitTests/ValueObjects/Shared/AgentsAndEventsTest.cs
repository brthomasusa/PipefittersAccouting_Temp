#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.UnitTests.ValueObjects.Shared
{
    public class AgentsAndEventsTest
    {
        [Fact]
        public void ShouldReturn_Valid_ExternalAgentType()
        {
            var result = ExternalAgentType.Create(AgentType.Employee);

            Assert.IsType<ExternalAgentType>(result);
            Assert.Equal(AgentType.Employee, result.AgentType);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_ExternalAgentType()
        {
            Action action = () => ExternalAgentType.Create(0);

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void ShouldReturn_Valid_EconomicEventType()
        {
            var result = EconomicEventType.Create(EventType.CashDisbursementForDividentPayment);

            Assert.IsType<EconomicEventType>(result);
            Assert.Equal(EventType.CashDisbursementForDividentPayment, result.EventType);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_EconomicEventType()
        {
            Action action = () => EconomicEventType.Create(0);

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);
        }
    }
}