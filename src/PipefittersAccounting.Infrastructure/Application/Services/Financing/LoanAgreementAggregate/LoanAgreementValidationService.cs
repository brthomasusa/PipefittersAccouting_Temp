using PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate
{
    public class LoanAgreementValidationService : ILoanAgreementValidationService
    {
        private IQueryServicesRegistry _servicesRegistry;

        public LoanAgreementValidationService
        (
            ILoanAgreementQueryService loanAgreementQrySvc,
            ISharedQueryService sharedQueryService,
            IQueryServicesRegistry servicesRegistry
        )
        {
            _servicesRegistry = servicesRegistry;

            _servicesRegistry.RegisterService("LoanAgreementQueryService", loanAgreementQrySvc);
            _servicesRegistry.RegisterService("SharedQueryService", sharedQueryService);
        }

        public async Task<ValidationResult> IsValidCreateLoanAgreement(LoanAgreementWriteModel writeModel)
        {
            CreateLoanAgreementValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidDeleteLoanAgreement(LoanAgreementWriteModel writeModel)
        {
            DeleteLoanAgreementValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }
    }
}