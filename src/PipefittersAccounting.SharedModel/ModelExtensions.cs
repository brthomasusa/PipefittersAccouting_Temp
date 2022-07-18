using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.SharedModel
{
    public static class ModelExtensions
    {
        public static CashTransactionWriteModel Map(this TimeCardPaymentInfo input, Guid cashAcctId, Guid userId)
        {
            return new CashTransactionWriteModel()
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
            return new FinancierWriteModel()
            {
                Id = input.FinancierId,
                FinancierName = input.FinancierName!,
                Telephone = input.Telephone!,
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
    }
}
