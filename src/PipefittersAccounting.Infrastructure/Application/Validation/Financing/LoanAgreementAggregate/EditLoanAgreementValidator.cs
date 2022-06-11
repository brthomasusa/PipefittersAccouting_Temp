using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate
{
    public class EditLoanAgreementValidator : ValidatorBase<LoanAgreementWriteModel, IQueryServicesRegistry>
    {
        public EditLoanAgreementValidator
        (
            LoanAgreementWriteModel writeModel,
            IQueryServicesRegistry queryServicesRegistry

        ) : base(writeModel, queryServicesRegistry)
        {

        }

        public override async Task<ValidationResult> Validate()
        {
            LoanAgreementQueryService loanAgreementQueryService
                = QueryServicesRegistry.GetService<LoanAgreementQueryService>("LoanAgreementQueryService");

            SharedQueryService sharedQueryService
                = QueryServicesRegistry.GetService<SharedQueryService>("SharedQueryService");

            VerifyAgentIsFinancierRule verifyCreditor = new(sharedQueryService);
            VerifyEventIsLoanAgreementRule verifyEventIsLoanAgreement = new(sharedQueryService);
            VerifyCreditorIsLinkedToLoanAgreementRule verifyCreditorLinkToAgreement = new(loanAgreementQueryService);
            VerifyLoanProceedsNotReceivedRule verifyDepositOfLoanProceeds = new(loanAgreementQueryService);

            verifyCreditor.SetNext(verifyEventIsLoanAgreement);
            verifyEventIsLoanAgreement.SetNext(verifyCreditorLinkToAgreement);
            verifyCreditorLinkToAgreement.SetNext(verifyDepositOfLoanProceeds);

            return await verifyCreditor.Validate(WriteModel);
        }
    }
}