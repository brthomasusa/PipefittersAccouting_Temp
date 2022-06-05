using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DepositOfStockIssueProceedsValidator : CashAccountTransactionValidationBase
    {
        public DepositOfStockIssueProceedsValidator
        (
            CashTransactionWriteModel deposit,
            ICashAccountQueryService queryService,
            ISharedQueryService sharedQueryService
        ) : base(deposit, queryService, sharedQueryService)
        {

        }

        public async override Task<ValidationResult> Validate()
        {
            // Check that financier is known to the system
            VerifyAgentIsFinancierRule agentValidator = new(SharedQueryService);

            // Check that stock subscription is known to the system
            VerifyEventIsStockSubscriptionRule eventValidator = new(SharedQueryService);

            // Ensure that loan agreement belongs to this financier
            VerifyCreditorHasLoanAgreementRule loanAgreementIssuedByFinancierValidator = new(CashAccountQueryService);

            // Verify that transaction date is between loan date and maturity
            // Verify that transaction amount equals loan agreement amount
            // Verify that this deposit has not already been made
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsRule miscDetailsValidator = new(CashAccountQueryService);

            agentValidator.SetNext(eventValidator);
            eventValidator.SetNext(loanAgreementIssuedByFinancierValidator);
            loanAgreementIssuedByFinancierValidator.SetNext(miscDetailsValidator);

            return await agentValidator.Validate(CashAccountTransactionInfo);
        }
    }
}