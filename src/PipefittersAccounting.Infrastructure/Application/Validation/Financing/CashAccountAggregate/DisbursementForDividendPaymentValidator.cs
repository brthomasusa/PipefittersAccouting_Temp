using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DisbursementForDividendPaymentValidator : CashAccountTransactionValidatorBase
    {
        public DisbursementForDividendPaymentValidator
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

            // Check that dividend declaration is known to the system
            VerifyEventIsDividendDeclarationRule eventRule = new(SharedQueryService);

            // Ensure that dividend declaration belongs to this investor
            VerifyInvestorHasDividendDeclarationRule investorDividendRule = new(CashAccountQueryService);

            // Verify that transaction date is after dividend declaration date.
            // Verify that transaction amount equals dividend per shares * shares issued
            // Verify that this deposit has not already been made
            VerifyDetailsOfDisbursementForDividendPaymentRule miscDetailsRule = new(CashAccountQueryService);

            agentRule.SetNext(eventRule);
            eventRule.SetNext(investorDividendRule);
            investorDividendRule.SetNext(miscDetailsRule);

            return await agentRule.Validate(CashAccountTransactionInfo);
        }
    }
}