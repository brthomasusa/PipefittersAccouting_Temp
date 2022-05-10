using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class CreateCashAccountInfoValidation
    {
        public static async Task<ValidationResult> Validate(CreateCashAccountInfo accountInfo, ICashAccountQueryService queryService)
        {
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(queryService);
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            return await acctNameValidator.Validate(accountInfo);
        }
    }
}