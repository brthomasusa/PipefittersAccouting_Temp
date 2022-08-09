#pragma warning disable CS8600
#pragma warning disable CS8604

using System;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.UnitTests.ValueObjects.HumanResources
{
    public class EmployeeAggregateTests
    {
        [Fact]
        public void ShouldReturn_Valid_Employee()
        {
            Employee employee = new Employee
            (
                EntityGuidID.Create(Guid.NewGuid()),
                EmployeeTypeEnum.Maintenance,
                EntityGuidID.Create(Guid.NewGuid()),
                PersonName.Create("Doe", "John", "T"),
                SocialSecurityNumber.Create("235981457"),
                EmailAddress.Create("john.doe@pipefitterssupply.com"),
                PhoneNumber.Create("214-874-1234"),
                Address.Create("123 Main Street", "Apt 25", "Dallas", "TX", "75211"),
                MaritalStatus.Create("S"),
                TaxExemption.Create(1),
                PayRate.Create(20M),
                StartDate.Create(new DateTime(2022, 2, 3)),
                true,
                false
            );

            Assert.IsType<Employee>(employee);
        }

        [Fact]
        public void ShouldReturn_Valid_Employee_NullCheck()
        {
            // Set any of the input params below to null and run test (it will fail).
            // Should get specific error message describing missing parameter

            Employee employee = new Employee
            (
                EntityGuidID.Create(Guid.NewGuid()),
                EmployeeTypeEnum.Administrator,
                EntityGuidID.Create(Guid.NewGuid()),
                PersonName.Create("Doe", "John", "T"),
                SocialSecurityNumber.Create("235981457"),
                EmailAddress.Create("john.doe@pipefitterssupply.com"),
                PhoneNumber.Create("214-874-1234"),
                Address.Create("123 Main Street", "Apt 25", "Dallas", "TX", "75211"),
                MaritalStatus.Create("S"),
                TaxExemption.Create(1),
                PayRate.Create(20M),
                StartDate.Create(new DateTime(2022, 2, 3)),
                true,
                false
            );

            Assert.IsType<Employee>(employee);
        }

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

        [Fact]
        public void ShouldUpdate_Employee_SupervisorId()
        {
            Guid newID = Guid.NewGuid();
            Employee employee = GetEmployee();

            Assert.NotEqual(employee.SupervisorId, newID);

            employee.UpdateSupervisorId(EntityGuidID.Create(newID));

            Assert.Equal(employee.SupervisorId, newID);
        }

        [Fact]
        public void ShouldUpdate_Employee_EmployeeName()
        {
            PersonName newName = PersonName.Create("Pence", "Michael", "C");
            Employee employee = GetEmployee();

            Assert.NotEqual(newName, employee.EmployeeName);

            employee.UpdateEmployeeName(newName);

            Assert.Equal(newName, employee.EmployeeName);
        }

        [Fact]
        public void ShouldUpdate_Employee_SSN()
        {
            SocialSecurityNumber newSSN = SocialSecurityNumber.Create("423598145");
            Employee employee = GetEmployee();

            Assert.NotEqual(newSSN, employee.SSN);

            employee.UpdateSSN(newSSN);

            Assert.Equal(newSSN, employee.SSN);
        }

        [Fact]
        public void ShouldUpdate_Employee_PhoneNumber()
        {
            PhoneNumber phoneNumber = PhoneNumber.Create("817-874-9999");
            Employee employee = GetEmployee();

            Assert.NotEqual(phoneNumber, employee.EmployeeTelephone);

            employee.UpdateEmployeePhoneNumber(phoneNumber);

            Assert.Equal(phoneNumber, employee.EmployeeTelephone);
        }

        [Fact]
        public void ShouldUpdate_Employee_Address()
        {
            Address address = Address.Create("999 9th Ave", "Apt 9", "Somewhere", "TX", "75211");
            Employee employee = GetEmployee();

            Assert.NotEqual(address, employee.EmployeeAddress);

            employee.UpdateEmployeeAddress(address);

            Assert.Equal(address, employee.EmployeeAddress);
        }

        [Fact]
        public void ShouldUpdate_Employee_MaritalStatus()
        {
            MaritalStatus status = MaritalStatus.Create("M");
            Employee employee = GetEmployee();

            Assert.NotEqual(status, employee.MaritalStatus);

            employee.UpdateMaritalStatus(status);

            Assert.Equal(status, employee.MaritalStatus);
        }

        [Fact]
        public void ShouldUpdate_Employee_TaxExemption()
        {
            TaxExemption exemption = TaxExemption.Create(10);
            Employee employee = GetEmployee();

            Assert.NotEqual(exemption, employee.TaxExemptions);

            employee.UpdateTaxExemptions(exemption);

            Assert.Equal(exemption, employee.TaxExemptions);
        }

        [Fact]
        public void ShouldUpdate_Employee_PayRate()
        {
            PayRate payRate = PayRate.Create(10.25M);
            Employee employee = GetEmployee();

            Assert.NotEqual(payRate, employee.EmployeePayRate);

            employee.UpdateEmployeePayRate(payRate);

            Assert.Equal(payRate, employee.EmployeePayRate);
        }

        [Fact]
        public void ShouldUpdate_Employee_StartDate()
        {
            StartDate startDate = StartDate.Create(new DateTime(2022, 2, 5));
            Employee employee = GetEmployee();

            Assert.NotEqual(startDate, employee.EmploymentDate);

            employee.UpdateEmploymentDate(startDate);

            Assert.Equal(startDate, employee.EmploymentDate);
        }

        [Fact]
        public void ShouldUpdate_Employee_IsActive()
        {
            Employee employee = GetEmployee();

            Assert.True(employee.IsActive);

            employee.UpdateEmployeeStatus(false);

            Assert.False(employee.IsActive);
        }

        [Fact]
        public void ShouldUpdate_Employee_IsSupervisor()
        {
            Employee employee = GetEmployee();

            Assert.False(employee.IsSupervisor);

            employee.UpdateIsSupervisor(true);

            Assert.True(employee.IsSupervisor);
        }

        private Employee GetEmployee() =>
            new Employee
            (
                EntityGuidID.Create(Guid.NewGuid()),
                EmployeeTypeEnum.Salesperson,
                EntityGuidID.Create(Guid.NewGuid()),
                PersonName.Create("Doe", "John", "T"),
                SocialSecurityNumber.Create("235981457"),
                EmailAddress.Create("john.doe@pipefitterssupply.com"),
                PhoneNumber.Create("214-874-1234"),
                Address.Create("123 Main Street", "Apt 25", "Dallas", "TX", "75211"),
                MaritalStatus.Create("S"),
                TaxExemption.Create(1),
                PayRate.Create(20M),
                StartDate.Create(new DateTime(2022, 2, 3)),
                true,
                false
            );

    }
}