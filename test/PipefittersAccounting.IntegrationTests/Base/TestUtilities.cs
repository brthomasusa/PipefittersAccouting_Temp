using System;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public static class TestUtilities
    {
        public static CreateEmployeeInfo GetCreateEmployeeInfo() =>
            new CreateEmployeeInfo()
            {
                Id = Guid.NewGuid(),
                SupervisorId = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E"),
                LastName = "Trump",
                FirstName = "Ivanka",
                MiddleInitial = "I",
                SSN = "434679876",
                Telephone = "555-555-5555",
                AddressLine1 = "139th Street NW",
                AddressLine2 = "B1",
                City = "Edison",
                StateCode = "NJ",
                Zipcode = "08837",
                MaritalStatus = "M",
                Exemptions = 3,
                PayRate = 25M,
                StartDate = new DateTime(2022, 2, 13),
                IsActive = true,
                IsSupervisor = false
            };

        public static EditEmployeeInfo GetEditEmployeeInfo() =>
            new EditEmployeeInfo()
            {
                Id = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5"),
                SupervisorId = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E"),
                LastName = "Trump",
                FirstName = "Ivanka",
                MiddleInitial = "I",
                SSN = "434679876",
                Telephone = "555-555-5555",
                AddressLine1 = "139th Street NW",
                AddressLine2 = "B1",
                City = "Edison",
                StateCode = "NJ",
                Zipcode = "08837",
                MaritalStatus = "M",
                Exemptions = 3,
                PayRate = 25M,
                StartDate = new DateTime(2022, 2, 13),
                IsActive = true,
                IsSupervisor = false
            };
    }
}