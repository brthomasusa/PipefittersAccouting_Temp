
using PipefittersAccounting.Infrastructure.Application.Queries.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateQueryService : IEmployeeAggregateQueryService
    {
        private readonly DapperContext _dapperCtx;

        public EmployeeAggregateQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters) =>
            await GetEmployeeDetailQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters) =>
            await GetEmployeeListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagersParameters queryParameters) =>
            await GetEmployeeManagersQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyEmployeeSSNIsUnique(UniqueEmployeSSNParameter queryParameters)
            => await VerifyEmployeeSSNIsUniqueQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyEmployeeNameIsUnique(UniqueEmployeeNameParameters queryParameters)
            => await VerifyEmployeeNameIsUniqueQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<int>> GetCountOfEmployeeTimeCards(GetEmployeeParameter queryParameters)
            => await GetCountOfEmployeeTimeCardsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<TimeCardDetail>> GetEmployeeTimeCardDetails(GetTimeCardParameter queryParameters)
            => await GetEmployeeTimeCardDetailQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<TimeCardListItem>>> GetEmployeeTimeCardListItems(GetEmployeeParameter queryParameters)
            => await GetEmployeeTimeCardListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyEmployeeSupervisorLink(GetEmployeeParameter queryParameters)
            => await VerifyEmployeeSupervisorLinkQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<DateTime>> GetMostRecentPayPeriodEndedDate(GetMostRecentPayPeriodParameter queryParameters)
            => await GetMostRecentPayPeriodEndedDateQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<TimeCardVerification>> VerifyTimeCardEvent(GetTimeCardParameter queryParameters)
            => await VerifyTimeCardEventQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<TimeCardPaymentVerification>> GetTimeCardPaymentVerification(GetTimeCardParameter queryParameters)
            => await GetTimeCardPaymentVerificationQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<PayrollRegister>>> GetPayrollRegister(GetPayrollRegisterParameter queryParameters)
            => await GetPayrollRegisterQuery.Query(queryParameters, _dapperCtx);
    }
}