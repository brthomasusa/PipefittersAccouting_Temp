using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public interface ICashReceiptForDebtIssueValidator
    {
        OperationResult<bool> Validate(CashTransaction transaction);
    }
}