using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DisbursementForSinglePayrollPymtValidator : ValidatorBase<PayrollRegisterWriteModel, IQueryServicesRegistry>
    {
        public DisbursementForSinglePayrollPymtValidator
        (
            PayrollRegisterWriteModel writeModel,
            IQueryServicesRegistry queryServicesRegistry

        ) : base(writeModel, queryServicesRegistry)
        {

        }

        public override async Task<ValidationResult> Validate()
        {
            CashAccountQueryService cashAccountQueryService
                = QueryServicesRegistry.GetService<CashAccountQueryService>("CashAccountQueryService");

            VerifySufficientFundsInPayrollAccountRule verifyAcctBalance = new(cashAccountQueryService);

            return await verifyAcctBalance.Validate(WriteModel);
        }
    }
}