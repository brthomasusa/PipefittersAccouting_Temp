#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules
{
    public class VerifySufficientFundsForMultiplePayrollChecksRule
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifySufficientFundsForMultiplePayrollChecksRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public async Task<ValidationResult> Validate(List<PayrollRegisterWriteModel> writeModels)
        {
            ValidationResult validationResult = new();
            decimal totalNetPay = 0;
            writeModels.ForEach(model => totalNetPay += model.NetPay);

            GetCashAccount queryParameters = new() { CashAccountId = writeModels[0].CashAccountId };

            OperationResult<CashAccountDetail> getResult =
                await _cashAcctQrySvc.GetCashAccountDetails(queryParameters);

            if (!getResult.Success)
            {
                string msg = $"A cash account with id '{writeModels[0].CashAccountId}' could not be located.";
                validationResult.Messages.Add(msg);
            }
            else
            {
                if (getResult.Result.Balance > totalNetPay)
                {
                    validationResult.IsValid = true;
                }
                else
                {
                    string msg = $"The cash account balance of {getResult.Result.Balance} is insufficient to cover the payroll net pay amount of {totalNetPay}.";
                    validationResult.Messages.Add(msg);
                }
            }

            return validationResult;
        }
    }
}