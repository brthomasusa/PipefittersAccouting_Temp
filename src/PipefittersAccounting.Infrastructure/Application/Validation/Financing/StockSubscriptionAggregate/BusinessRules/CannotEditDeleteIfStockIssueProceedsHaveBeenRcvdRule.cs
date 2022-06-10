#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules
{
    public class CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule : BusinessRule<StockSubscriptionWriteModel>
    {
        private readonly IStockSubscriptionQueryService _qrySvc;

        public CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule(IStockSubscriptionQueryService cashAcctQrySvc)
            => _qrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(StockSubscriptionWriteModel subscriptionInfo)
        {
            ValidationResult validationResult = new();
            GetStockSubscriptionParameter queryParameters =
                new()
                {
                    StockId = subscriptionInfo.StockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _qrySvc.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            if (result.Success)
            {
                if (result.Result.AmountReceived == 0)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(subscriptionInfo);
                    }
                }
                else
                {
                    string msg = $"Cannot edit or delete stock subscription if stock issue proceeds have been deposited! {result.Result.AmountReceived} was deposited on {result.Result.DateReceived}.";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}