using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class NewCashAccountNameMustBeUniqueValidator : Validator<CreateCashAccountInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public NewCashAccountNameMustBeUniqueValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountInfo cashAccount)
        {
            ValidationResult validationResult = new();

            GetCashAccountWithAccountName queryParams = new() { AccountName = cashAccount.CashAccountName };

            OperationResult<CashAccountReadModel> getResult =
                await _cashAcctQrySvc.GetCashAccountWithAccountName(queryParams);

            if (getResult.Success)
            {
                string msg = $"There is an existing cash account with account name '{cashAccount.CashAccountName}'";
                validationResult.IsValid = false;
                validationResult.Messages.Add(msg);
            }
            else
            {
                validationResult.IsValid = true;
                validationResult = await base.Validate(cashAccount);
            }

            return validationResult;
        }
    }
}