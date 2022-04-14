using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashReceiptForDebtIssueValidator : ICashReceiptForDebtIssueValidator
    {
        public OperationResult<bool> Validate(CashTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}