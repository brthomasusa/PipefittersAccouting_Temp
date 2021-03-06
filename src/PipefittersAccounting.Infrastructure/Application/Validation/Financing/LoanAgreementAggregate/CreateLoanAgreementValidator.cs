using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate
{
    public class CreateLoanAgreementValidator : ValidatorBase<LoanAgreementWriteModel, IQueryServicesRegistry>
    {
        public CreateLoanAgreementValidator
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
            VerifyLoanAgreementIsNotDuplicateRule verifyNotDuplicateRule = new(loanAgreementQueryService);

            verifyCreditor.SetNext(verifyNotDuplicateRule);

            return await verifyCreditor.Validate(WriteModel);
        }
    }
}