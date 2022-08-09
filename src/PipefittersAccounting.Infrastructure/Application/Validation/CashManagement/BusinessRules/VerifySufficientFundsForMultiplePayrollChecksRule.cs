#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class VerifySufficientFundsForMultiplePayrollChecksRule
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifySufficientFundsForMultiplePayrollChecksRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public async Task<ValidationResult> Validate(List<CashTransactionWriteModel> writeModels)
        {
            ValidationResult validationResult = new();
            decimal totalNetPay = 0;
            writeModels.ForEach(model => totalNetPay += model.TransactionAmount);

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