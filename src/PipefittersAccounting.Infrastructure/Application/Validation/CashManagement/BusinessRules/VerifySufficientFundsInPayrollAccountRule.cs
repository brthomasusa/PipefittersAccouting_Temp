#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class VerifySufficientFundsInPayrollAccountRule : BusinessRule<PayrollRegisterWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifySufficientFundsInPayrollAccountRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(PayrollRegisterWriteModel writeModel)
        {
            ValidationResult validationResult = new();

            GetCashAccount queryParameters = new() { CashAccountId = writeModel.CashAccountId };

            OperationResult<CashAccountDetail> getResult =
                await _cashAcctQrySvc.GetCashAccountDetails(queryParameters);

            if (!getResult.Success)
            {
                string msg = $"A cash account with id '{writeModel.CashAccountId}' could not be located.";
                validationResult.Messages.Add(msg);
            }
            else
            {
                if (getResult.Result.Balance > writeModel.NetPay)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(writeModel);
                    }
                }
                else
                {
                    string msg = $"The cash account balance of {getResult.Result.Balance} is insufficient to cover the payroll net pay amount of {writeModel.NetPay}.";
                    validationResult.Messages.Add(msg);
                }
            }

            return validationResult;
        }
    }
}