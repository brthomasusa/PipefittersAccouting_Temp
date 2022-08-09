using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class NewCashAccountNumberMustBeUniqueRule : BusinessRule<CashAccountWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public NewCashAccountNumberMustBeUniqueRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashAccountWriteModel cashAccount)
        {
            ValidationResult validationResult = new();

            GetCashAccountWithAccountNumber queryParams = new() { AccountNumber = cashAccount.CashAccountNumber };

            OperationResult<CashAccountReadModel> getResult =
                await _cashAcctQrySvc.GetCashAccountWithAccountNumber(queryParams);

            if (getResult.Success)
            {
                string msg = $"There is an existing cash account with account number '{cashAccount.CashAccountNumber}'";
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