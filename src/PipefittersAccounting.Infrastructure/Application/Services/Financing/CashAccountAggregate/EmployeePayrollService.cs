using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate
{
    public class EmployeePayrollService
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public EmployeePayrollService(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        //  Method
    }
}