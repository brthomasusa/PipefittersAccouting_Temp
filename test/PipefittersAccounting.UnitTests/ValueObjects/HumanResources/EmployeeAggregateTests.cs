#pragma warning disable CS8600
#pragma warning disable CS8604

using System;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.UnitTests.ValueObjects.HumanResources
{
    public class EmployeeAggregateTests
    {
        [Fact]
        public void ShouldReturn_Valid_EmployeePayRate()
        {
            decimal rate = 7.51M;

            var result = PayRate.Create(rate);

            Assert.IsType<PayRate>(result);
            Assert.Equal(rate, result.Value);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_PayRateZero()
        {
            decimal rate = 0;

            Action action = () => PayRate.Create(rate);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid pay rate; pay rate can not be zero!", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_PayRateLessThanMinWage()
        {
            decimal rate = 7.49M;

            Action action = () => PayRate.Create(rate);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid pay rate, must be between $7.50 and $40.00 (per hour) inclusive!", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_PayRateTooLarge()
        {
            decimal rate = 40.01M;

            Action action = () => PayRate.Create(rate);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid pay rate, must be between $7.50 and $40.00 (per hour) inclusive!", caughtException.Message);
        }

        [Fact]
        public void ShouldReturn_Valid_TaxExemption()
        {
            int exemption = 0;

            var result = TaxExemption.Create(exemption);

            Assert.IsType<TaxExemption>(result);
            Assert.Equal(exemption, result);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_NegativeTaxExemption()
        {
            int exemption = -1;

            Action action = () => TaxExemption.Create(exemption);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Number of exemptions must be greater than or equal to zero.", caughtException.Message);
        }

        [Fact]
        public void ShouldReturn_Valid_StartDate()
        {
            DateTime start = new DateTime(2021, 6, 1);

            var result = StartDate.Create(start);

            Assert.IsType<StartDate>(result);
            Assert.Equal(start, result);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_StartDateIsDefaultDate()
        {
            DateTime start = new DateTime();

            Action action = () => StartDate.Create(start);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Contains("The employee start date is required.", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_StartDate_Before_19980101()
        {
            DateTime start = new DateTime(1997, 12, 31);

            Action action = () => StartDate.Create(start);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void ShouldReturnValid_SSN()
        {
            string ssn = "587887964";

            var result = SocialSecurityNumber.Create(ssn);

            Assert.IsType<SocialSecurityNumber>(result);
            Assert.Equal(ssn, result);
        }

        [Fact]
        public void ShouldReturn_Valid_MaritalStatus()
        {
            string status = "M";

            var result = MaritalStatus.Create(status);

            Assert.IsType<MaritalStatus>(result);
            Assert.Equal(status, result.Value);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_MaritalStatusNotMorS()
        {
            string status = "D";

            Action action = () => MaritalStatus.Create(status);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid marital status, valid statues are 'S' and 'M'.", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_MaritalStatusIsNull()
        {
            string status = null;

            Action action = () => MaritalStatus.Create(status);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Contains("The marital status is required.", caughtException.Message);
        }
    }
}