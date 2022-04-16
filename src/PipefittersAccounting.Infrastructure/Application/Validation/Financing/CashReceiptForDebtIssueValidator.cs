using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class CashReceiptForDebtIssueValidator
    {
        public static async Task<ValidationResult> Validate(CashTransaction transaction, ICashAccountQueryService queryService)
        {
            ValidationResult validationResult = new();

            FinancierIdValidationParams financierParam = new() { FinancierId = transaction.AgentId };
            OperationResult<FinancierIdValidationModel> financierResult = await queryService.GetFinancierIdValidationModel(financierParam);

            if (financierResult.Success)
            {
                CreditorHasLoanAgreeValidationParams loanAgreementParam = new() { FinancierId = transaction.AgentId, LoanId = transaction.EventId };
                OperationResult<CreditorHasLoanAgreeValidationModel> loanAgreementResult =
                    await queryService.GetCreditorHasLoanAgreeValidationModel(loanAgreementParam);

                if (loanAgreementResult.Success)
                {
                    ReceiptLoanProceedsValidationParams loanAmountParam = new() { FinancierId = transaction.AgentId, LoanId = transaction.EventId };
                    OperationResult<ReceiptLoanProceedsValidationModel> loanAmountResult =
                        await queryService.GetReceiptLoanProceedsValidationModel(loanAmountParam);

                    if (loanAmountResult.Success)
                    {
                        if (loanAmountResult.Result.LoanAmount == 0)
                        {
                            validationResult.IsValid = true;
                        }
                        else
                        {
                            validationResult.Messages.Add("The loan proceed amount and the loan agreement amount do not match.");
                        }
                    }
                    else
                    {
                        validationResult.Messages.Add(loanAmountResult.NonSuccessMessage);
                    }
                }
                else
                {
                    validationResult.Messages.Add(loanAgreementResult.NonSuccessMessage);
                }
            }
            else
            {
                validationResult.Messages.Add(financierResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}