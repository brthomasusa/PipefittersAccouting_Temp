using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;


namespace PipefittersAccounting.SharedModel
{
    public static class ModelExtensions
    {
        public static CashTransactionWriteModel Map(this TimeCardPaymentInfo input, Guid cashAcctId, Guid userId)
        {
            return new()
            {
                CashAccountId = cashAcctId,
                TransactionType = 6,
                TransactionDate = input.PayPeriodEnded,
                TransactionAmount = input.NetPay,
                AgentId = input.EmployeeId,
                EventId = input.TimeCardId,
                RemittanceAdvice = $"Payroll: {input.EmployeeName}, period ended: {input.PayPeriodEnded}",
                UserId = userId
            };
        }

        public static FinancierWriteModel Map(this FinancierReadModel input)
        {
            return new()
            {
                Id = input.FinancierId,
                FinancierName = input.FinancierName!,
                Telephone = input.Telephone!,
                EmailAddress = input.EmailAddress!,
                AddressLine1 = input.AddressLine1!,
                AddressLine2 = input.AddressLine2!,
                City = input.City!,
                StateCode = input.StateCode!,
                Zipcode = input.Zipcode!,
                ContactFirstName = input.ContactFirstName!,
                ContactLastName = input.ContactLastName!,
                ContactMiddleInitial = input.ContactMiddleInitial!,
                ContactTelephone = input.ContactTelephone!,
                IsActive = input.IsActive,
                UserId = input.UserId
            };
        }

        public static EmployeeWriteModel Map(this EmployeeDetail input)
        {
            return new()
            {
                EmployeeId = input.EmployeeId,
                SupervisorId = input.SupervisorId,
                EmployeeType = input.EmployeeTypeId,
                FirstName = input.FirstName!,
                LastName = input.LastName!,
                MiddleInitial = input.MiddleInitial!,
                Telephone = input.Telephone!,
                EmailAddress = input.EmailAddress!,
                SSN = input.SSN!,
                AddressLine1 = input.AddressLine1!,
                AddressLine2 = input.AddressLine2!,
                City = input.City!,
                StateCode = input.StateCode!,
                Zipcode = input.Zipcode!,
                MaritalStatus = input.MaritalStatus!,
                Exemptions = input.Exemptions,
                PayRate = input.PayRate,
                StartDate = input.StartDate,
                IsActive = input.IsActive,
                IsSupervisor = input.IsSupervisor
            };
        }

        public static TimeCardWriteModel Map(this TimeCardWithPymtInfo input)
        {
            return new()
            {
                TimeCardId = input.TimeCardId,
                EmployeeId = input.EmployeeId,
                SupervisorId = input.SupervisorId,
                PayPeriodEnded = input.PayPeriodEnded,
                RegularHours = input.RegularHours,
                OvertimeHours = input.OvertimeHours,
                UserId = input.UserId
            };
        }

        public static LoanAgreementWriteModel Map(this LoanAgreementReadModel input)
        {
            return new()
            {
                LoanId = input.LoanId,
                FinancierId = input.FinancierId,
                LoanAmount = input.LoanAmount,
                InterestRate = input.InterestRate,
                LoanDate = input.LoanDate,
                MaturityDate = input.MaturityDate,
                NumberOfInstallments = input.NumberOfInstallments,
                UserId = input.UserId
            };
        }

        public static LoanInstallmentWriteModel Map(this LoanInstallmentReadModel input)
        {
            return new()
            {
                LoanInstallmentId = input.LoanInstallmentId,
                LoanId = input.LoanId,
                InstallmentNumber = input.InstallmentNumber,
                PaymentDueDate = input.PaymentDueDate,
                PaymentAmount = input.EqualMonthlyInstallment,
                PrincipalPymtAmount = input.PrincipalAmount,
                InterestPymtAmount = input.InterestAmount,
                PrincipalRemaining = input.PrincipalRemaining,
                UserId = input.UserId
            };
        }

        public static StockSubscriptionWriteModel Map(this StockSubscriptionReadModel input)
        {
            return new()
            {
                StockId = input.StockId,
                FinancierId = input.FinancierId,
                StockIssueDate = input.StockIssueDate,
                SharesIssued = input.SharesIssured,
                PricePerShare = input.PricePerShare,
                UserId = input.UserId
            };
        }
    }
}
