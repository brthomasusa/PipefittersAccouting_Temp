using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class CannotEditOrDeleteCashAcctWithTransactionsRule : BusinessRule<CashAccountWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public CannotEditOrDeleteCashAcctWithTransactionsRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashAccountWriteModel cashAccount)
        {
            ValidationResult validationResult = new();

            GetCashAccount queryParams = new() { CashAccountId = cashAccount.CashAccountId };

            OperationResult<int> getResult =
                await _cashAcctQrySvc.GetNumberOfCashAccountTransactions(queryParams);

            if (getResult.Success)
            {
                if (getResult.Result == 0)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(cashAccount);
                    }
                }
                else
                {
                    string msg = $"This cash account has {getResult.Result} transactions. It can not be deleted.";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(getResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}