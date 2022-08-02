#pragma warning disable CS8625

using System;
using System.Collections.Generic;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public class EmployeeAggregateTestData
    {
        public static EmployeeWriteModel GetEmployeeWriteModelCreate() =>
            new EmployeeWriteModel()
            {
                EmployeeId = Guid.NewGuid(),
                EmployeeType = 1,
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
                PayRate = 25.00M,
                StartDate = new DateTime(2022, 2, 13),
                IsActive = true,
                IsSupervisor = false
            };

        public static EmployeeWriteModel GetEmployeeWriteModelEdit() =>
            new EmployeeWriteModel()
            {
                EmployeeId = new Guid("aedc617c-d035-4213-b55a-dae5cdfca366"),
                EmployeeType = 5,
                SupervisorId = new Guid("4b900a74-e2d9-4837-b9a4-9e828752716e"),
                LastName = "Goldberg",
                FirstName = "Jozef",
                MiddleInitial = "P",
                SSN = "036889999",
                Telephone = "469-321-1234",
                AddressLine1 = "6667 Melody Lane",
                AddressLine2 = "Apt 2",
                City = "Dallas",
                StateCode = "TX",
                Zipcode = "75231",
                MaritalStatus = "S",
                Exemptions = 1,
                PayRate = 29.00M,
                StartDate = new DateTime(2013, 2, 28),
                IsActive = true,
                IsSupervisor = true
            };

        public static EmployeeWriteModel GetEmployeeWriteModel_WayneCarter() =>
            new EmployeeWriteModel()
            {
                EmployeeId = new Guid("5c60f693-bef5-e011-a485-80ee7300c695"),
                EmployeeType = 3,
                SupervisorId = new Guid("4b900a74-e2d9-4837-b9a4-9e828752716e"),
                LastName = "Carter",
                FirstName = "Wayne",
                MiddleInitial = "L",
                SSN = "423789999",
                Telephone = "972-523-1234",
                AddressLine1 = "321 Fort Worth Ave",
                City = "Dallas",
                StateCode = "TX",
                Zipcode = "75211",
                MaritalStatus = "M",
                Exemptions = 3,
                PayRate = 40.00M,
                StartDate = new DateTime(2013, 2, 28),
                IsActive = true,
                IsSupervisor = true
            };

        public static TimeCardWriteModel GetTimeCardForCreate() =>
            new TimeCardWriteModel()
            {
                TimeCardId = EntityGuidID.Create(new Guid("f9fedab5-668d-4f08-b4d2-7fb4d464f252")),
                EmployeeId = EntityGuidID.Create(new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52")),
                SupervisorId = EntityGuidID.Create(new Guid("aedc617c-d035-4213-b55a-dae5cdfca366")),
                PayPeriodEnded = new DateTime(2022, 3, 31),
                RegularHours = 184,
                OvertimeHours = 40,
                UserId = EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            };

        public static TimeCardWriteModel GetTimeCardForEdit() =>
            new TimeCardWriteModel()
            {
                TimeCardId = EntityGuidID.Create(new Guid("d4ad0ad8-7e03-4bb2-8ce0-04e5e95428a1")),
                EmployeeId = EntityGuidID.Create(new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52")),
                SupervisorId = EntityGuidID.Create(new Guid("4b900a74-e2d9-4837-b9a4-9e828752716e")),
                PayPeriodEnded = new DateTime(2022, 2, 28),
                RegularHours = 180,
                OvertimeHours = 0,
                UserId = EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            };

        public static List<PayrollRegister> GetPayrollRegister_01312022() =>
            new List<PayrollRegister>()
            {
                new PayrollRegister()
                {
                TimeCardId = new Guid("175a1bc8-dbba-41bb-98af-7377f1f64d07"),
                EmployeeId = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5"),
                EmployeeName = "Jeffery W Beck",
                RegularPay = 5040.00M,
                OvertimePay = 90.00M,
                GrossPay = 5130.00M,
                FICA = 318.06M,
                Medicare = 74.39M,
                FederalWithholding = 308.17M,
                NetPay = 4429.38M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("1626daa5-5acb-40e9-8907-eb25db991583"),
                EmployeeId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788"),
                EmployeeName = "Jamie J Brown",
                RegularPay = 4611.00M,
                OvertimePay = 87.00M,
                GrossPay = 4698.00M,
                FICA = 291.28M,
                Medicare = 68.12M,
                FederalWithholding = 380.25M,
                NetPay = 3958.35M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("2d325646-0d70-4e2e-b458-0ca43a916fab"),
                EmployeeId = new Guid("e716ac28-e354-4d8d-94e4-ec51f08b1af8"),
                EmployeeName = "George W Bush",
                RegularPay = 5040.00M,
                OvertimePay = 0,
                GrossPay = 5040.00M,
                FICA = 312.48M,
                Medicare = 73.08M,
                FederalWithholding = 294.67M,
                NetPay = 4359.77M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("3a386b77-361b-49e0-89aa-b806b24bf333"),
                EmployeeId = new Guid("5c60f693-bef5-e011-a485-80ee7300c695"),
                EmployeeName = "Wayne L Carter",
                RegularPay = 6600.00M,
                OvertimePay = 120.00M,
                GrossPay = 6720.00M,
                FICA = 416.64M,
                Medicare = 97.44M,
                FederalWithholding = 637.94M,
                NetPay = 5567.98M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("bda7e369-c21f-4b99-a492-08c7a30d0a4b"),
                EmployeeId = new Guid("9f7b902d-566c-4db6-b07b-716dd4e04340"),
                EmployeeName = "Terri L Duffy",
                RegularPay = 4800.00M,
                OvertimePay = 45.00M,
                GrossPay = 4845.00M,
                FICA = 300.39M,
                Medicare = 70.25M,
                FederalWithholding = 402.30M,
                NetPay = 4072.06M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("0e104001-ed28-40d7-92f3-462f93d45b4a"),
                EmployeeId = new Guid("8b140613-5df8-4f57-beb4-e3f5cd45ad3c"),
                EmployeeName = "Gail A Erickson",
                RegularPay = 3361.50M,
                OvertimePay = 0,
                GrossPay = 3361.50M,
                FICA = 208.41M,
                Medicare = 48.74M,
                FederalWithholding = 309.82M,
                NetPay = 2794.53M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("a1e05c99-fa99-485d-b4ca-719a55e01482"),
                EmployeeId = new Guid("aedc617c-d035-4213-b55a-dae5cdfca366"),
                EmployeeName = "Jozef P Goldberg",
                RegularPay = 4872.00M,
                OvertimePay = 217.50M,
                GrossPay = 5089.50M,
                FICA = 315.55M,
                Medicare = 73.80M,
                FederalWithholding = 791.48M,
                NetPay = 3908.67M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("4383b5fc-d6fc-4475-a22d-b7b2e17f75bb"),
                EmployeeId = new Guid("9d3a25dc-3861-4f78-92b0-92294b808ebf"),
                EmployeeName = "Diane W Margheim",
                RegularPay = 3024.00M,
                OvertimePay = 0,
                GrossPay = 3024.00M,
                FICA = 187.49M,
                Medicare = 43.85M,
                FederalWithholding = 304.82M,
                NetPay = 2487.84M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("29dc4ad2-5e5a-4f08-bb3d-468abf57a10e"),
                EmployeeId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"),
                EmployeeName = "Terri M Phide",
                RegularPay = 4704.00M,
                OvertimePay = 126.00M,
                GrossPay = 4830.00M,
                FICA = 299.46M,
                Medicare = 70.04M,
                FederalWithholding = 445.67M,
                NetPay = 4014.83M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("63e13c5d-4586-461f-be78-3e240d625bbf"),
                EmployeeId = new Guid("604536a1-e734-49c4-96b3-9dfef7417f9a"),
                EmployeeName = "Ma A Rainey",
                RegularPay = 4578.00M,
                OvertimePay = 0,
                GrossPay = 4578.00M,
                FICA = 283.84M,
                Medicare = 66.38M,
                FederalWithholding = 362.25M,
                NetPay = 3865.53M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("748fcab5-9464-4d5f-937f-d61ffe811e6f"),
                EmployeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53"),
                EmployeeName = "Sharon C Salavaria",
                RegularPay = 3024.00M,
                OvertimePay = 108.00M,
                GrossPay = 3132.00M,
                FICA = 194.18M,
                Medicare = 45.41M,
                FederalWithholding = 321.02M,
                NetPay = 2571.39M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("40cdffa1-965e-4e3a-8a47-8cb542c2ef64"),
                EmployeeId = new Guid("4b900a74-e2d9-4837-b9a4-9e828752716e"),
                EmployeeName = "Ken J Sanchez",
                RegularPay = 6720.00M,
                OvertimePay = 0,
                GrossPay = 6720.00M,
                FICA = 416.64M,
                Medicare = 97.44M,
                FederalWithholding = 546.67M,
                NetPay = 5659.25M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("11b9d933-3007-4759-9110-f3e1a20ac71f"),
                EmployeeId = new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52"),
                EmployeeName = "Roberto W Tamburello",
                RegularPay = 3612.00M,
                OvertimePay = 32.25M,
                GrossPay = 3644.25M,
                FICA = 225.94M,
                Medicare = 52.84M,
                FederalWithholding = 0,
                NetPay = 3365.47M
                },
            };

        public static List<PayrollRegister> GetPayrollRegister_02282022() =>
            new List<PayrollRegister>()
            {
                new PayrollRegister()
                {
                TimeCardId = new Guid("8df9c13f-3d60-4bc4-ba3e-5bc2ceadf2c9"),
                EmployeeId = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5"),
                EmployeeName = "Jeffery W Beck",
                RegularPay = 5040.00M,
                OvertimePay = 45.00M,
                GrossPay = 5085.00M,
                FICA = 315.27M,
                Medicare = 7373M,
                FederalWithholding = 301.42M,
                NetPay = 4394.58M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("00bc7e3c-a5cb-46ec-8a19-6966b769e8e0"),
                EmployeeId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788"),
                EmployeeName = "Jamie J Brown",
                RegularPay = 4611.00M,
                OvertimePay = 0M,
                GrossPay = 4611.00M,
                FICA = 285.88M,
                Medicare = 66.86M,
                FederalWithholding = 367.20M,
                NetPay = 3891.06M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("3ae463a6-7437-41a9-9e99-ec4d7f29fc89"),
                EmployeeId = new Guid("e716ac28-e354-4d8d-94e4-ec51f08b1af8"),
                EmployeeName = "George W Bush",
                RegularPay = 5040.00M,
                OvertimePay = 0,
                GrossPay = 5040.00M,
                FICA = 312.48M,
                Medicare = 73.08M,
                FederalWithholding = 294.67M,
                NetPay = 4359.77M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("a093b4a9-0d38-4a1c-b8e4-375416ebae14"),
                EmployeeId = new Guid("5c60f693-bef5-e011-a485-80ee7300c695"),
                EmployeeName = "Wayne L Carter",
                RegularPay = 6720.00M,
                OvertimePay = 00M,
                GrossPay = 6720.00M,
                FICA = 416.64M,
                Medicare = 97.44M,
                FederalWithholding = 637.94M,
                NetPay = 5567.98M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("5c90925f-ec82-43f4-b8f0-9c058a7e1664"),
                EmployeeId = new Guid("9f7b902d-566c-4db6-b07b-716dd4e04340"),
                EmployeeName = "Terri L Duffy",
                RegularPay = 5040.00M,
                OvertimePay = 0M,
                GrossPay = 5040.00M,
                FICA = 312.48M,
                Medicare = 73.08M,
                FederalWithholding = 431.55M,
                NetPay = 4222.89M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("0aa0d9dc-572f-4ae9-bb07-0cdab0a8f06d"),
                EmployeeId = new Guid("8b140613-5df8-4f57-beb4-e3f5cd45ad3c"),
                EmployeeName = "Gail A Erickson",
                RegularPay = 3402.00M,
                OvertimePay = 0,
                GrossPay = 3402.00M,
                FICA = 210.92M,
                Medicare = 49.33M,
                FederalWithholding = 315.90M,
                NetPay = 2825.85M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("f7f54340-4838-4709-9c5f-8737164aa727"),
                EmployeeId = new Guid("aedc617c-d035-4213-b55a-dae5cdfca366"),
                EmployeeName = "Jozef P Goldberg",
                RegularPay = 4843.00M,
                OvertimePay = 0M,
                GrossPay = 4843.00M,
                FICA = 300.27M,
                Medicare = 70.22M,
                FederalWithholding = 729.86M,
                NetPay = 3742.65M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("061ba320-1572-4889-aea0-7c97d0e1f1a8"),
                EmployeeId = new Guid("9d3a25dc-3861-4f78-92b0-92294b808ebf"),
                EmployeeName = "Diane W Margheim",
                RegularPay = 3024.00M,
                OvertimePay = 0,
                GrossPay = 3024.00M,
                FICA = 187.49M,
                Medicare = 43.85M,
                FederalWithholding = 304.82M,
                NetPay = 2487.84M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("a7501599-71ea-4164-818c-c2478fdf7872"),
                EmployeeId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"),
                EmployeeName = "Terri M Phide",
                RegularPay = 4704.00M,
                OvertimePay = 0M,
                GrossPay = 4704.00M,
                FICA = 291.65M,
                Medicare = 68.21M,
                FederalWithholding = 426.77M,
                NetPay = 3917.37M
                },

                new PayrollRegister()
                {
                TimeCardId = new Guid("c1221fe9-f04f-4d10-a6cf-802359a26a84"),
                EmployeeId = new Guid("604536a1-e734-49c4-96b3-9dfef7417f9a"),
                EmployeeName = "Ma A Rainey",
                RegularPay = 4578.00M,
                OvertimePay = 0,
                GrossPay = 4578.00M,
                FICA = 283.84M,
                Medicare = 66.38M,
                FederalWithholding = 362.25M,
                NetPay = 3865.53M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("e2b5406c-dbd5-405a-8f38-f23943f2e32f"),
                EmployeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53"),
                EmployeeName = "Sharon C Salavaria",
                RegularPay = 3024.00M,
                OvertimePay = 0M,
                GrossPay = 3024.00M,
                FICA = 187.49M,
                Medicare = 43.85M,
                FederalWithholding = 304.82M,
                NetPay = 2487.84M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("5e7d00e1-8aad-43ce-a5e6-690341bef644"),
                EmployeeId = new Guid("4b900a74-e2d9-4837-b9a4-9e828752716e"),
                EmployeeName = "Ken J Sanchez",
                RegularPay = 6720.00M,
                OvertimePay = 0,
                GrossPay = 6720.00M,
                FICA = 416.64M,
                Medicare = 97.44M,
                FederalWithholding = 546.67M,
                NetPay = 5659.25M
                },
                new PayrollRegister()
                {
                TimeCardId = new Guid("d4ad0ad8-7e03-4bb2-8ce0-04e5e95428a1"),
                EmployeeId = new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52"),
                EmployeeName = "Roberto W Tamburello",
                RegularPay = 3612.00M,
                OvertimePay = 32.25M,
                GrossPay = 3644.25M,
                FICA = 225.94M,
                Medicare = 52.84M,
                FederalWithholding = 0,
                NetPay = 3365.47M
                },
            };
    }
}

