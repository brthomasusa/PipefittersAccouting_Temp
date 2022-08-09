using PipefittersAccounting.Infrastructure.Application.Services.CashManagement;
using PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement
{
    public class DisbursementForMultiplePayrollPymtValidator
    {
        private IQueryServicesRegistry _queryServicesRegistry;
        private List<CashTransactionWriteModel> _writeModels;

        public DisbursementForMultiplePayrollPymtValidator
        (
            List<CashTransactionWriteModel> writeModels,
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