using PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement
{
    public class DepositOfStockIssueProceedsValidator : CashAccountTransactionValidatorBase
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

            // Ensure that stock subscription belongs to this investor
            VerifyInvestorHasStockSubscriptionRule investorStockSubscriptionValidator = new(CashAccountQueryService);

            // Verify that transaction date is after dividend declaration date.
            // Verify that transaction amount equals dividend per shares * shares issued
            // Verify that this deposit has not already been made
            VerifyDetailsOfDepositOfStockIssueProceedsRule miscDetailsValidator = new(CashAccountQueryService);

            agentValidator.SetNext(eventValidator);
            eventValidator.SetNext(investorStockSubscriptionValidator);
            investorStockSubscriptionValidator.SetNext(miscDetailsValidator);

            return await agentValidator.Validate(CashAccountTransactionInfo);
        }
    }
}