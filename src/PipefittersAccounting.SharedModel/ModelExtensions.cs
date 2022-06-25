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
    }
}