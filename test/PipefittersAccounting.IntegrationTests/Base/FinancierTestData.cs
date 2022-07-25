#pragma warning disable CS8625

using System;
using System.Collections.Generic;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public static class FinancierTestData
    {
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
        public static FinancierWriteModel GetCreateFinancierInfo() =>
            new FinancierWriteModel()
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

        public static FinancierWriteModel GetEditFinancierInfo() =>
            new FinancierWriteModel()
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

        public static FinancierWriteModel GetDeleteFinancierInfo() =>
            new FinancierWriteModel()
            {
                Id = new Guid("84164388-28ff-4b47-bd63-dd9326d32236"),
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
    }
}