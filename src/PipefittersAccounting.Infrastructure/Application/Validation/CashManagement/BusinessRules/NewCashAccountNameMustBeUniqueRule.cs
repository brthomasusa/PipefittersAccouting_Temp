using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class NewCashAccountNameMustBeUniqueRule : BusinessRule<CashAccountWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public NewCashAccountNameMustBeUniqueRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashAccountWriteModel cashAccount)
        {
            ValidationResult validationResult = new();

            GetCashAccountWithAccountName queryParams = new() { AccountName = cashAccount.CashAccountName };

            OperationResult<CashAccountReadModel> getResult =
                await _cashAcctQrySvc.GetCashAccountWithAccountName(queryParams);

            if (getResult.Success)
            {
                string msg = $"There is an existing cash account with account name '{cashAccount.CashAccountName}'";
                validationResult.Messages.Add(msg);
            }
            else
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next.Validate(cashAccount);
                }
            }

            return validationResult;
        }
    }
}