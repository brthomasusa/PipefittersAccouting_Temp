using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Application.Commands.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.CashManagement
{
    public class EmployeePayrollService : IEmployeePayrollService
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;
        private readonly IEmployeeAggregateQueryService _employeeQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeePayrollService
        (
            ICashAccountQueryService cashAcctQrySvc,
            IEmployeeAggregateQueryService employeeQrySvc,
            ICashAccountAggregateValidationService validationService,
            ICashAccountAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            _cashAcctQrySvc = cashAcctQrySvc;
            _employeeQrySvc = employeeQrySvc;
            _validationService = validationService;
            _repository = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> Process(Guid cashAccountId, Guid userId)
        {
            GetMostRecentPayPeriodParameter payPeriodParameter = new() { };
            OperationResult<DateTime> payPeriodResult = await _employeeQrySvc.GetMostRecentPayPeriodEndedDate(payPeriodParameter);

            if (payPeriodResult.Success)
            {
                if (payPeriodResult.Result != default)
                {
                    GetTimeCardPaymentInfoParameter queryParameters =
                        new()
                        {
                            PayPeriodBegin = new DateTime(payPeriodResult.Result.Year, payPeriodResult.Result.Month, 1),
                            PayPeriodEnd = payPeriodResult.Result
                        };

                    OperationResult<List<TimeCardPaymentInfo>> result =
                        await _cashAcctQrySvc.GetTimeCardPaymentInfo(queryParameters);

                    if (result.Success)
                    {
                        CreatePayrollCashTransactions createPayrollCashTransactions
                            = new(cashAccountId, userId, result.Result);

                        CreateDisbursementsForMultipleTimeCardsCommand command = new(createPayrollCashTransactions, _repository, _validationService, _unitOfWork);
                        OperationResult<bool> createResult = await command.Process();

                        if (createResult.Success)
                        {
                            return OperationResult<bool>.CreateSuccessResult(true);
                        }
                        else
                        {
                            return OperationResult<bool>.CreateFailure(createResult.NonSuccessMessage);
                        }
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                    }
                }
                else
                {
                    string msg = "Unable to determine starting pay period date.";
                    return OperationResult<bool>.CreateFailure(msg);
                }
            }
            else
            {
                return OperationResult<bool>.CreateFailure(payPeriodResult.NonSuccessMessage);
            }
        }
    }
}