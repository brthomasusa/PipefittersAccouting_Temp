#pragma warning disable CS8625

using System;
using System.Collections.Generic;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public static class TestUtilities
    {
        public static Employee GetNewEmployee() =>
            new Employee
            (
                EntityGuidID.Create(Guid.NewGuid()),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Dough", "Jonnie", "J"),
                SocialSecurityNumber.Create("123789901"),
                PhoneNumber.Create("817-987-1234"),
                Address.Create("123 Main Plaza", "Unit 54", "Fort Worth", "TX", "78965"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(5),
                PayRate.Create(40M),
                StartDate.Create(new DateTime(1998, 12, 2)),
                true,
                true
            );


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

        public static Financier GetFinancierForCreating() =>
            new Financier
            (
                EntityGuidID.Create(Guid.NewGuid()),
                OrganizationName.Create("Wide World Financiers"),
                PhoneNumber.Create("817-235-9874"),
                Address.Create("666 Trump Plaza", "Ste 666", "Fort Worth", "TX", "78965"),
                PointOfContact.Create("Ivanka", "Trump", "I", "817-235-9874"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                true
            );

        public static Financier GetFinancierForEditing() =>
            new Financier
            (
                    EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),
                    OrganizationName.Create("Arturo Sandoval"),
                    PhoneNumber.Create("888-719-8128"),
                    Address.Create("5232 Outriggers Way", "Ste 401", "Oxnard", "CA", "93035"),
                    PointOfContact.Create("Arturo", "Sandoval", "T", "888-719-8128"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
            );

        public static List<Financier> GetFinanciers() =>
            new List<Financier>()
            {
                new Financier
                (
                    EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),
                    OrganizationName.Create("Arturo Sandoval"),
                    PhoneNumber.Create("888-719-8128"),
                    Address.Create("5232 Outriggers Way", "Ste 401", "Oxnard", "CA", "93035"),
                    PointOfContact.Create("Arturo", "Sandoval", "T", "888-719-8128"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
                ),
                new Financier
                (
                    EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601")),
                    OrganizationName.Create("Paul Van Horn Enterprises"),
                    PhoneNumber.Create("415-328-9870"),
                    Address.Create("825 Mandalay Beach Rd", "Level 2", "Oxnard", "CA", "94402"),
                    PointOfContact.Create("Patrick", "Crocker", "T", "415-328-9870"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
                ),
                new Financier
                (
                    EntityGuidID.Create(new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886")),
                    OrganizationName.Create("New World Tatoo Parlor"),
                    PhoneNumber.Create("630-321-9875"),
                    Address.Create("1690 S. El Camino Real", "Room 2C", "San Mateo", "CA", "94402"),
                    PointOfContact.Create("JoJo", "Jozef Jr.", "D", "630-321-9875"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
                ),
                new Financier
                (
                    EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
                    OrganizationName.Create("Bertha Mae Jones Innovative Financing"),
                    PhoneNumber.Create("886-587-0001"),
                    Address.Create("12333 Menard Heights Blvd", "Ste 1001", "Palo Alto", "CA", "94901"),
                    PointOfContact.Create("Betty", "Sinosky", "L", "886-587-0001"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
                ),
                new Financier
                (
                    EntityGuidID.Create(new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d")),
                    OrganizationName.Create("Pimps-R-US Financial Management, Inc."),
                    PhoneNumber.Create("415-931-5570"),
                    Address.Create("96541 Sunset Rise Plaza", "Ste 2", "Oxnard", "CA", "93035"),
                    PointOfContact.Create("Javier", "Daniels", "A", "415-931-5570"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
                ),
                new Financier
                (
                    EntityGuidID.Create(new Guid("84164388-28ff-4b47-bd63-dd9326d32236")),
                    OrganizationName.Create("I Exist-Only-To-Be-Deleted"),
                    PhoneNumber.Create("415-912-5570"),
                    Address.Create("985211 Highway 78 East", null, "Oxnard", "CA", "93035"),
                    PointOfContact.Create("Monica", "Gutierrez", "T", "415-912-5570"),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                    true
                )
            };
        public static CreateFinancierInfo GetCreateFinancierInfo() =>
            new CreateFinancierInfo()
            {
                Id = Guid.NewGuid(),
                FinancierName = "The Deep Pockets Group",
                Telephone = "555-555-5555",
                AddressLine1 = "139th Street NW",
                AddressLine2 = "B1",
                City = "Edison",
                StateCode = "NJ",
                Zipcode = "08837",
                ContactFirstName = "Ivanka",
                ContactLastName = "Trump",
                ContactMiddleInitial = "I",
                ContactTelephone = "555-555-5555",
                IsActive = true,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static EditFinancierInfo GetEditFinancierInfo() =>
            new EditFinancierInfo()
            {
                Id = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
                FinancierName = "Bertha Mae Jones Down Home Cooking",
                Telephone = "886-587-0001",
                AddressLine1 = "12333 Menard Heights Blvd",
                AddressLine2 = "Suite 1001",
                City = "Palo Alto",
                StateCode = "CA",
                Zipcode = "94901",
                ContactFirstName = "Betty",
                ContactLastName = "Sinosky",
                ContactMiddleInitial = "L",
                ContactTelephone = "886-587-0001",
                IsActive = true,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static DeleteFinancierInfo GetDeleteFinancierInfo() =>
            new DeleteFinancierInfo()
            {
                Id = new Guid("84164388-28ff-4b47-bd63-dd9326d32236"),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };


        // public static LoanAgreement GetLoanAgreementForCreating() =>
        //     new LoanAgreement
        //     (
        //         LoanAgreementEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
        //         EntityGuidID.Create(new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886")),
        //         LoanAmount.Create(33000),
        //         InterestRate.Create(.0725M),
        //         LoanDate.Create(new DateTime(2021, 11, 15)),
        //         MaturityDate.Create(new DateTime(2022, 11, 15)),
        //         NumberOfInstallments.Create(12),
        //         EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
        //         LoanAgreementTestData.GetInstallmentsLoanRepymtScheduleUpdate()
        //     );

        // public static LoanAgreement GetLoanAgreementForEditing() =>
        //     new LoanAgreement
        //     (
        //         LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"))),
        //         EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
        //         LoanAmount.Create(33000),
        //         InterestRate.Create(.0725M),
        //         LoanDate.Create(new DateTime(2021, 11, 15)),
        //         MaturityDate.Create(new DateTime(2022, 10, 15)),
        //         NumberOfInstallments.Create(12),
        //         EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
        //         GetInstallmentsValidInfo()
        //     );

        public static List<InstallmentRecord> GetInstallmentsValidInfo()
        {
            List<InstallmentRecord> pymtSchedule = new();

            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 15),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 15),
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 451.00M,
                                Interest: 4.65M,
                                TotalInterestPaid: 19.95M,
                                RemainingBalance: 453.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 15),
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }









    }
}