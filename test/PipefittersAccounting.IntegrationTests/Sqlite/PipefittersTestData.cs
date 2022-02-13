#pragma warning disable CS8600
#pragma warning disable CS8625

using System;
using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Financing.FinancierAggregate;

namespace PipefittersAccounting.IntegrationTests.Sqlite
{
    public static class PipefittersTestData
    {
        public static void SeedDatabase(this AppDbContext context)
        {
            InsertEventTypes(context);
            InsertAgentTypes(context);
            InsertExternalAgentAndEmployees(context);
            InsertDomainUsers(context);
            InsertExternalAgentAndFinanciers(context);
        }

        private static void InsertEventTypes(AppDbContext ctx)
        {
            string sql =
            @"
            INSERT INTO EconomicEventTypes
                (EventTypeName, CreatedDate)
            VALUES
                ('Cash Receipt from Sales', '2022-02-11 00:00:00'),
                ('Cash Receipt from Loan Agreement', '2022-02-11 00:00:00'),
                ('Cash Receipt from Stock Subscription', '2022-02-11 00:00:00'),     
                ('Cash Disbursement for Loan Payment', '2022-02-11 00:00:00'),
                ('Cash Disbursement for Divident Payment', '2022-02-11 00:00:00'),
                ('Cash Disbursement for TimeCard Payment', '2022-02-11 00:00:00'),
                ('Cash Disbursement for Inventory Receipt', '2022-02-11 00:00:00')           
            ";

            ctx.Database.ExecuteSqlRaw(sql);
        }

        private static void InsertAgentTypes(AppDbContext ctx)
        {
            string sql =
            @"
            INSERT INTO ExternalAgentTypes
                (AgentTypeName, CreatedDate)
            VALUES
                ('Customer', '2022-02-11 00:00:00'),
                ('Creditor', '2022-02-11 00:00:00'),
                ('Stockholder', '2022-02-11 00:00:00'),
                ('Vendor', '2022-02-11 00:00:00'),
                ('Employee', '2022-02-11 00:00:00'),
                ('Financier', '2022-02-11 00:00:00')          
            ";

            ctx.Database.ExecuteSqlRaw(sql);
        }

        private static void InsertExternalAgentAndEmployees(AppDbContext ctx)
        {
            Employee employee1 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Sanchez", "Ken", "J"),
                SocialSecurityNumber.Create("123789999"),
                PhoneNumber.Create("817-987-1234"),
                Address.Create("321 Tarrant Pl", null, "Fort Worth", "TX", "78965"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(5),
                PayRate.Create(40M),
                StartDate.Create(new DateTime(1998, 12, 2)),
                true,
                true
            );
            ctx.Employees.Add(employee1);

            Employee employee2 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("5C60F693-BEF5-E011-A485-80EE7300C695"))),
                EntityGuidID.Create(new Guid("e716ac28-e354-4d8d-94e4-ec51f08b1af8")),
                PersonName.Create("Carter", "Wayne", "L"),
                SocialSecurityNumber.Create("423789999"),
                PhoneNumber.Create("972-523-1234"),
                Address.Create("321 Fort Worth Ave", null, "Dallas", "TX", "75211"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(3),
                PayRate.Create(30M),
                StartDate.Create(new DateTime(2005, 12, 2)),
                true,
                false
            );
            ctx.Employees.Add(employee2);

            Employee employee3 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("604536a1-e734-49c4-96b3-9dfef7417f9a"))),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                PersonName.Create("Rainey", "Ma", "A"),
                SocialSecurityNumber.Create("775559874"),
                PhoneNumber.Create("903-555-5555"),
                Address.Create("1233 Back Alley Rd", null, "Corsicana", "TX", "75110"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(2),
                PayRate.Create(27.25M),
                StartDate.Create(new DateTime(2018, 1, 5)),
                true,
                false
            );
            ctx.Employees.Add(employee3);

            Employee employee4 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Phide", "Terri", "M"),
                SocialSecurityNumber.Create("638912345"),
                PhoneNumber.Create("214-987-1234"),
                Address.Create("3455 South Corinth Circle", null, "Dallas", "TX", "75224"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(1),
                PayRate.Create(28M),
                StartDate.Create(new DateTime(2014, 9, 22)),
                true,
                true
            );
            ctx.Employees.Add(employee4);

            Employee employee5 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("9f7b902d-566c-4db6-b07b-716dd4e04340"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Duffy", "Terri", "L"),
                SocialSecurityNumber.Create("699912345"),
                PhoneNumber.Create("214-987-1234"),
                Address.Create("98 Reiger Ave", null, "Dallas", "TX", "75214"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(2),
                PayRate.Create(30M),
                StartDate.Create(new DateTime(2018, 10, 22)),
                true,
                false
            );
            ctx.Employees.Add(employee5);

            Employee employee6 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("AEDC617C-D035-4213-B55A-DAE5CDFCA366"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Goldberg", "Jozef", "P"),
                SocialSecurityNumber.Create("036889999"),
                PhoneNumber.Create("469-321-1234"),
                Address.Create("6667 Melody Lane", "Apt 2090", "Dallas", "TX", "75231"),
                MaritalStatus.Create("S"),
                TaxExemption.Create(1),
                PayRate.Create(29M),
                StartDate.Create(new DateTime(2013, 2, 28)),
                true,
                false
            );
            ctx.Employees.Add(employee6);

            Employee employee7 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Brown", "Jamie", "J"),
                SocialSecurityNumber.Create("123700009"),
                PhoneNumber.Create("817-555-5555"),
                Address.Create("98777 Nigeria Town Rd", null, "Arlington", "TX", "78658"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(2),
                PayRate.Create(29M),
                StartDate.Create(new DateTime(2017, 12, 22)),
                true,
                false
            );
            ctx.Employees.Add(employee7);

            Employee employee8 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("e716ac28-e354-4d8d-94e4-ec51f08b1af8"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Bush", "George", "W"),
                SocialSecurityNumber.Create("325559874"),
                PhoneNumber.Create("214-555-5555"),
                Address.Create("777 Ervay Street", null, "Dallas", "TX", "75208"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(5),
                PayRate.Create(30M),
                StartDate.Create(new DateTime(2016, 10, 19)),
                true,
                true
            );
            ctx.Employees.Add(employee8);

            Employee employee9 = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5"))),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Beck", "Jeffery", "W"),
                SocialSecurityNumber.Create("825559874"),
                PhoneNumber.Create("214-555-5555"),
                Address.Create("321 Fort Worth Ave", null, "Dallas", "TX", "75211"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(5),
                PayRate.Create(30M),
                StartDate.Create(new DateTime(2015, 11, 12)),
                true,
                false
            );
            ctx.Employees.Add(employee9);

            ctx.SaveChanges();
        }

        private static void InsertDomainUsers(AppDbContext ctx)
        {
            ExternalAgent agent =
                ctx.ExternalAgents.Find(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                    ?? throw new ArgumentException("Unable to locate agent with Id: '4b900a74-e2d9-4837-b9a4-9e828752716e'.");

            ctx.DomainUsers.Add(new DomainUser(agent, "tphide", "terri.phide@pipefitterssupplycompany.com"));
            ctx.SaveChanges();
        }

        private static void InsertExternalAgentAndFinanciers(AppDbContext ctx)
        {
            DomainUser user = ctx.DomainUsers.Find(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                ?? throw new ArgumentException("Unable to locate domain user with Id: '660bb318-649e-470d-9d2b-693bfb0b2744'."); ;

            Financier financier1 = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695"))),
                OrganizationName.Create("Arturo Sandoval"),
                PhoneNumber.Create("888-719-8128"),
                Address.Create("5232 Outriggers Way", "Ste 401", "Oxnard", "CA", "93035"),
                PointOfContact.Create(PersonName.Create("Sandoval", "Arturo", "T"), PhoneNumber.Create("888-719-8128")),
                EntityGuidID.Create(user.Id),
                true
            );
            ctx.Financiers.Add(financier1);

            Financier financier2 = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"))),
                OrganizationName.Create("Paul Van Horn Enterprises"),
                PhoneNumber.Create("415-328-9870"),
                Address.Create("825 Mandalay Beach Rd", "Level 2", "Oxnard", "CA", "94402"),
                PointOfContact.Create(PersonName.Create("Crocker", "Patrick", "T"), PhoneNumber.Create("415-328-9870")),
                EntityGuidID.Create(user.Id),
                true
            );
            ctx.Financiers.Add(financier2);

            Financier financier3 = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886"))),
                OrganizationName.Create("New World Tatoo Parlor"),
                PhoneNumber.Create("630-321-9875"),
                Address.Create("1690 S. El Camino Real", "Room 2C", "San Mateo", "CA", "94402"),
                PointOfContact.Create(PersonName.Create("Jozef Jr.", "JoJo", "D"), PhoneNumber.Create("630-321-9875")),
                EntityGuidID.Create(user.Id),
                true
            );
            ctx.Financiers.Add(financier3);

            Financier financier4 = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"))),
                OrganizationName.Create("Bertha Mae Jones Innovative Financing"),
                PhoneNumber.Create("886-587-0001"),
                Address.Create("12333 Menard Heights Blvd", "Ste 1001", "Palo Alto", "CA", "94901"),
                PointOfContact.Create(PersonName.Create("Sinosky", "Betty", "L"), PhoneNumber.Create("886-587-0001")),
                EntityGuidID.Create(user.Id),
                true
            );
            ctx.Financiers.Add(financier4);

            Financier financier5 = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d"))),
                OrganizationName.Create("Pimps-R-US Financial Management, Inc."),
                PhoneNumber.Create("415-931-5570"),
                Address.Create("96541 Sunset Rise Plaza", "Ste 2", "Oxnard", "CA", "93035"),
                PointOfContact.Create(PersonName.Create("Daniels", "Javier", "A"), PhoneNumber.Create("415-931-5570")),
                EntityGuidID.Create(user.Id),
                true
            );
            ctx.Financiers.Add(financier5);

            Financier financier6 = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(new Guid("84164388-28ff-4b47-bd63-dd9326d32236"))),
                OrganizationName.Create("I Exist-Only-To-Be-Deleted"),
                PhoneNumber.Create("415-912-5570"),
                Address.Create("985211 Highway 78 East", null, "Oxnard", "CA", "93035"),
                PointOfContact.Create(PersonName.Create("Gutierrez", "Monica", "T"), PhoneNumber.Create("415-912-5570")),
                EntityGuidID.Create(user.Id),
                true
            );
            ctx.Financiers.Add(financier6);

            ctx.SaveChanges();
        }
    }
}