using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    /*
        This class arranges a group of validators into a chain.
        Execution of the chain will completely validate a
        CreateCashAccountTransactionInfo. The position of each
        validator within the chain is important. In other words,
        the sequence in which the validation is performed is
        important.
    */

    public class DepositOfDebtIssueProceedsValidation : BaseCashAccountTransactionValidation
    {
        public DepositOfDebtIssueProceedsValidation
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
            // Check that financier is known to the system
            FinancierAsExternalAgentRule agentValidator = new(QueryService);

            // Check that loan agreement is known to the system
            LoanAgreementAsEconomicEventRule eventValidator = new(QueryService);

            // Ensure that loan agreement belongs to this financier
            IsCreditorAssociatedWithThisLoanAgreeRule loanAgreementIssuedByFinancierValidator = new(QueryService);

            // Verify that transaction date is between loan date and maturity
            // Verify that transaction amount equals loan agreement amount
            // Verify that this deposit has not already been made
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsRule miscDetailsValidator = new(QueryService);

            agentValidator.SetNext(eventValidator);
            eventValidator.SetNext(loanAgreementIssuedByFinancierValidator);
            loanAgreementIssuedByFinancierValidator.SetNext(miscDetailsValidator);

            return await agentValidator.Validate(CashAccountTransactionInfo);
        }
    }
}