#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    // Using Dapper ORM to verify the existence of a cash account
    public class SourceCashAccountRule : BusinessRule<CashAccountTransferWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public SourceCashAccountRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashAccountTransferWriteModel transferInfo)
        {
            ValidationResult validationResult = new();

            if (transferInfo.SourceCashAccountId != transferInfo.DestinationCashAccountId)
            {
                GetCashAccount queryParameters = new() { CashAccountId = transferInfo.SourceCashAccountId };

                OperationResult<CashAccountReadModel> getResult =
                    await _cashAcctQrySvc.GetCashAccountReadModel(queryParameters);

                if (!getResult.Success)
                {
                    string msg = $"The source cash account with id '{transferInfo.SourceCashAccountId}' could not be located.";
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