#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class DestinationCashAccountRule : BusinessRule<CashAccountTransferWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public DestinationCashAccountRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashAccountTransferWriteModel transferInfo)
        {
            ValidationResult validationResult = new();

            if (transferInfo.SourceCashAccountId != transferInfo.DestinationCashAccountId)
            {
                GetCashAccount queryParameters = new() { CashAccountId = transferInfo.DestinationCashAccountId };

                OperationResult<CashAccountReadModel> getResult =
                    await _cashAcctQrySvc.GetCashAccountReadModel(queryParameters);

                if (!getResult.Success)
                {
                    string msg = $"The destination cash account with id '{transferInfo.DestinationCashAccountId}' could not be located.";
                    validationResult.Messages.Add(msg);
                }
                else
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(transferInfo);
                    }
                }
            }
            else
            {
                string msg = "Invalid transfer; the source and destination accounts must be different.";
                validationResult.Messages.Add(msg);
            }


            return validationResult;
        }
    }
}