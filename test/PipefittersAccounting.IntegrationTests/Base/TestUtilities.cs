using System;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public static class TestUtilities
    {
        public static EmployeeDetail GetEmployeeDetail() =>
            new()
            {
                EmployeeId = new Guid("aedc617c-d035-4213-b55a-dae5cdfca366"),
                SupervisorId = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E"),
                ManagerLastName = "Sanchez",
                ManagerFirstName = "Ken",
                ManagerMiddleInitial = "J",
                ManagerFullName = "Ken J Sanchez",
                LastName = "Goldberg",
                FirstName = "Jozef",
                MiddleInitial = "P",
                EmployeeFullName = "Jozef P Goldberg",
                SSN = "036889999",
                Telephone = "469-321-1234",
                AddressLine1 = "6667 Melody Lane",
                AddressLine2 = "Apt 2",
                City = "Dallas",
                StateCode = "TX",
                Zipcode = "75231",
                MaritalStatus = "S",
                Exemptions = 1,
                PayRate = 29M,
                StartDate = new DateTime(2013, 2, 28),
                IsActive = true,
                IsSupervisor = false
            };

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
                LastName = "Beck",
                FirstName = "Jeffery",
                MiddleInitial = "W",
                SSN = "825559874",
                Telephone = "214-555-5555",
                AddressLine1 = "321 Fort Worth Ave",
                AddressLine2 = "B1",
                City = "Dallas",
                StateCode = "TX",
                Zipcode = "75211",
                MaritalStatus = "M",
                Exemptions = 5,
                PayRate = 30M,
                StartDate = new DateTime(2016, 10, 19),
                IsActive = true,
                IsSupervisor = false
            };

        public static DeleteEmployeeInfo GetDeleteEmployeeInfo() =>
            new DeleteEmployeeInfo()
            {
                Id = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5")
            };
    }
}