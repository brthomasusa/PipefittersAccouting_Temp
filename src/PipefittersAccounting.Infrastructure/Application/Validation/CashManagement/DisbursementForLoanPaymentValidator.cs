using PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement
{
    public class DisbursementForLoanPaymentValidator : CashAccountTransactionValidatorBase
    {
        public DisbursementForLoanPaymentValidator
        (
            CashTransactionWriteModel deposit,
            ICashAccountQueryService queryService,
            ISharedQueryService sharedQueryService
        ) : base(deposit, queryService, sharedQueryService)
        {
            CashAccountTransactionInfo = deposit;
            CashAccountQueryService = queryService;
        }

        public async override Task<ValidationResult> Validate()
        {
            // Check that loan installment is known to the system
            VerifyEventIsLoanInstallmentRule eventValidator = new(SharedQueryService);

            // Check that the financier is valid payee for this loan installment
            VerifyCreditorIsOwedLoanInstallmentRule payeeValidator = new(CashAccountQueryService);

            // Verify that debt issue proceeds have been received.
            VerifyDebtIssueProceedsHaveBeenReceivedRule verifyProceedsReceivedValidator = new(CashAccountQueryService);

            // Verify that transaction date is between loan date and maturity
            // Verify that transaction amount equals installment's EMI
            // Verify that this installment has not already been paid
            VerifyLoanPymtDateAmountAndStatusRule disburesementValidator = new(CashAccountQueryService);

            eventValidator.SetNext(payeeValidator);
            payeeValidator.SetNext(verifyProceedsReceivedValidator);
            verifyProceedsReceivedValidator.SetNext(disburesementValidator);

            return await eventValidator.Validate(CashAccountTransactionInfo);
        }
    }
}