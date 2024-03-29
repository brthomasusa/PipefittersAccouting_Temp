using PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement
{
    /*
        This class arranges a group of rules into a chain.
        Execution of the chain will completely validate a
        CashTransactionWriteModel. The position of each
        rule within the chain is important. In other words,
        the sequence in which rules are validated is
        important.
    */

    public class DepositOfDebtIssueProceedsValidator : CashAccountTransactionValidatorBase
    {
        public DepositOfDebtIssueProceedsValidator
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
            VerifyAgentIsFinancierRule agentRule = new(SharedQueryService);

            // Check that loan agreement is known to the system
            VerifyEventIsLoanAgreementRule eventRule = new(SharedQueryService);

            // Ensure that loan agreement belongs to this financier
            VerifyCreditorHasLoanAgreementRule loanAgreementIssuedByFinancierRule = new(CashAccountQueryService);

            // Verify that transaction date is between loan date and maturity
            // Verify that transaction amount equals loan agreement amount
            // Verify that this deposit has not already been made
            VerifyDetailsOfDepositOfDebtIssueProceedsRule miscDetailsRule = new(CashAccountQueryService);

            agentRule.SetNext(eventRule);
            eventRule.SetNext(loanAgreementIssuedByFinancierRule);
            loanAgreementIssuedByFinancierRule.SetNext(miscDetailsRule);

            return await agentRule.Validate(CashAccountTransactionInfo);
        }
    }
}