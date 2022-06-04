using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class CreateCashAccountInfoValidation
    {
        public static async Task<ValidationResult> Validate(CashAccountWriteModel accountInfo, ICashAccountQueryService queryService)
        {
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(queryService);
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            return await acctNameValidator.Validate(accountInfo);
        }
    }
}