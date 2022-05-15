using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class EditCashAccountInfoValidation
    {
        public static async Task<ValidationResult> Validate(EditCashAccountInfo accountInfo, ICashAccountQueryService queryService)
        {
            EditedCashAccountNameMustBeUniqueValidator acctNameValidator = new(queryService);

            // Check if user is attempting to update CashAccountType,  
            // which can only be done if cash account has no transactions
            GetCashAccount queryParams = new() { CashAccountId = accountInfo.CashAccountId };
            OperationResult<CashAccountReadModel> getResult =
                await queryService.GetCashAccountReadModel(queryParams);

            if (getResult.Success)
            {
                if (getResult.Result.CashAccountTypeId != accountInfo.CashAccountType)
                {
                    CannotEditCashAcctAcctTypeIfTransactionsExistValidator transactionCountValidator = new(queryService);
                    acctNameValidator.SetNext(transactionCountValidator);
                }
            }
            else    // Unable to retrieve count of transactions from database
            {
                ValidationResult validationResult = new();
                validationResult.Messages.Add(getResult.NonSuccessMessage);
                return validationResult;
            }

            return await acctNameValidator.Validate(accountInfo);
        }
    }
}