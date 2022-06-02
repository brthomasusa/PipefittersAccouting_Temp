using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class CannotEditDeleteDividendDeclarationIfPaidRule : BusinessRule<DividendDeclarationWriteModel>
    {
        private readonly IStockSubscriptionQueryService _qrySvc;

        public CannotEditDeleteDividendDeclarationIfPaidRule(IStockSubscriptionQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(DividendDeclarationWriteModel dividendInfo)
        {
            ValidationResult validationResult = new();
            GetDividendDeclarationParameters queryParameters =
                new()
                {
                    DividendId = dividendInfo.DividendId
                };

            OperationResult<VerifyCashDisbursementForDividendPayment> result =
                await _qrySvc.VerifyCashDisbursementDividendPayment(queryParameters);

            if (result.Success)
            {
                if (result.Result.StockId == dividendInfo.StockId)
                {
                    if (result.Result.AmountPaid == 0)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next.Validate(dividendInfo);
                        }
                    }
                    else
                    {
                        string msg = $"Multiple stock subscription Id's {result.Result.StockId} and {dividendInfo.StockId} detected.";
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    string msg = $"Cannot edit or delete dividend declaration if dividend has been paid! {result.Result.AmountPaid} was paid on {result.Result.DatePaid}.";
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