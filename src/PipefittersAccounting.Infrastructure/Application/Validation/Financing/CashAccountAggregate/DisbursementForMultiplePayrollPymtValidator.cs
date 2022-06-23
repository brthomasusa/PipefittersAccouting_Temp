using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DisbursementForMultiplePayrollPymtValidator
    {
        private IQueryServicesRegistry _queryServicesRegistry;
        private List<PayrollRegisterWriteModel> _writeModels;

        public DisbursementForMultiplePayrollPymtValidator
        (
            List<PayrollRegisterWriteModel> writeModels,
            IQueryServicesRegistry queryServicesRegistry

        )
        {
            _queryServicesRegistry = queryServicesRegistry;
            _writeModels = writeModels;
        }

        public async Task<ValidationResult> Validate()
        {
            CashAccountQueryService cashAccountQueryService
                = _queryServicesRegistry.GetService<CashAccountQueryService>("CashAccountQueryService");

            VerifySufficientFundsForMultiplePayrollChecksRule verifyAcctBalance = new(cashAccountQueryService);

            return await verifyAcctBalance.Validate(_writeModels);
        }
    }
}