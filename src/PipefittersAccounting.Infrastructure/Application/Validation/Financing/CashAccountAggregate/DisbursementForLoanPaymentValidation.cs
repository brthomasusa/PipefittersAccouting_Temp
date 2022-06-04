using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DisbursementForLoanPaymentValidation : BaseCashAccountTransactionValidation
    {
        public DisbursementForLoanPaymentValidation
        (
            CashTransactionWriteModel deposit,
            ICashAccountQueryService queryService
        ) : base(deposit, queryService)
        {
            CashAccountTransactionInfo = deposit;
            QueryService = queryService;
        }

        public async override Task<ValidationResult> Validate()
        {
            // Check that loan installment is known to the system
            LoanInstallmentPaymentAsEconomicEventRule eventValidator = new(QueryService);

            // Check that the financier is valid payee for this loan installment
            FinancierHasLoanInstallmentRule payeeValidator = new(QueryService);

            // Verify that debt issue proceeds have been received.
            VerifyDebtIssueProceedsHaveBeenReceivedRule verifyProceedsReceivedValidator = new(QueryService);

            // Verify that transaction date is between loan date and maturity
            // Verify that transaction amount equals installment's EMI
            // Verify that this installment has not already been paid
            DisburesementForLoanPymtRule disburesementValidator = new(QueryService);

            eventValidator.SetNext(payeeValidator);
            payeeValidator.SetNext(verifyProceedsReceivedValidator);
            verifyProceedsReceivedValidator.SetNext(disburesementValidator);

            return await eventValidator.Validate(CashAccountTransactionInfo);
        }
    }
}