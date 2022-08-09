using PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement
{
    public class CreateCashAccountInfoValidator
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