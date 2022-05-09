using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class NewCashAccountNumberMustBeUniqueValidator : Validator<CreateCashAccountInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public NewCashAccountNumberMustBeUniqueValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountInfo cashAccount)
        {
            ValidationResult validationResult = new();

            GetCashAccountWithAccountNumber queryParams = new() { AccountNumber = cashAccount.CashAccountNumber };

            OperationResult<CashAccountReadModel> getResult =
                await _cashAcctQrySvc.GetCashAccountWithAccountNumber(queryParams);

            if (getResult.Success)
            {
                string msg = $"There is an existing cash account with account number '{cashAccount.CashAccountNumber}'";
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