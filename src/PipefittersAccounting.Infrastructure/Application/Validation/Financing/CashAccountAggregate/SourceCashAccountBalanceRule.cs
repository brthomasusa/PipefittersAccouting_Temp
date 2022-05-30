#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class SourceCashAccountBalanceRule : BusinessRule<CashAccountTransferWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public SourceCashAccountBalanceRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashAccountTransferWriteModel transferInfo)
        {
            ValidationResult validationResult = new();

            GetCashAccount queryParameters = new() { CashAccountId = transferInfo.SourceCashAccountId };

            OperationResult<CashAccountDetail> getResult =
                await _cashAcctQrySvc.GetCashAccountDetails(queryParameters);

            if (!getResult.Success)
            {
                string msg = $"The source cash account with id '{transferInfo.SourceCashAccountId}' could not be located.";
                validationResult.Messages.Add(msg);
            }
            else
            {
                if (getResult.Result.Balance > transferInfo.CashTransferAmount)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(transferInfo);
                    }
                }
                else
                {
                    string msg = $"The source account balance of {getResult.Result.Balance} is insufficient to cover the transfer amount of {transferInfo.CashTransferAmount}.";
                    validationResult.Messages.Add(msg);
                }
            }

            return validationResult;
        }
    }
}