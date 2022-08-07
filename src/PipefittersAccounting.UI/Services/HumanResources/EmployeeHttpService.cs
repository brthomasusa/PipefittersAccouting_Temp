using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class EmployeeHttpService : IEmployeeHttpService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public EmployeeHttpService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // Alter system state with WriteModels
        public async Task<OperationResult<EmployeeDetail>> CreateEmployeeInfo(EmployeeWriteModel writeModel)
            => await CreateEmployeeHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel)
            => await EditEmployeeHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel)
            => await DeleteEmployeeHttpClient.Execute(writeModel, _client, _options);

        public Task<OperationResult<bool>> CreateTimeCardInfo(TimeCardWriteModel writeModel)
            => throw new NotImplementedException();

        public async Task<OperationResult<bool>> EditTimeCardInfo(TimeCardWriteModel writeModel)
           => await EditTimeCardHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> DeleteTimeCardInfo(TimeCardWriteModel writeModel)
            => await DeleteTimeCardHttpClient.Execute(writeModel, _client, _options);

        // Query system state with ReadModels        
        public async Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters)
            => await GetEmployeeDetailHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters)
            => await GetEmployeeListItemsHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesByLastNameParameters queryParameters)
            => await GetEmployeeListItemsHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers()
            => await GetManagersHttpClient.Query(_client, _options);

        public async Task<OperationResult<List<EmployeeTypes>>> GetEmployeeTypes()
            => await GetEmployeeTypesHttpClient.Query(_client, _options);

        public Task<OperationResult<TimeCardDetail>> GetEmployeeTimeCardDetails(GetTimeCardParameter queryParameters)
            => throw new NotImplementedException();

        public Task<OperationResult<List<TimeCardListItem>>> GetEmployeeTimeCardListItems(GetEmployeeParameter queryParameters)
            => throw new NotImplementedException();

        public async Task<OperationResult<List<PayrollRegister>>> GetPayrollRegister(GetPayrollRegisterParameter queryParameters)
            => await GetPayrollRegisterHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<List<TimeCardWithPymtInfo>>> GetTimeCardsForManager(GetTimeCardsForManagerParameter queryParameters)
            => await GetTimeCardsForManagerHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<List<TimeCardWithPymtInfo>>> GetTimeCardsForPayPeriod(GetTimeCardsForPayPeriodParameter queryParameters)
            => await GetTimeCardsForPayPeriodHttpClient.Query(queryParameters, _client, _options);
    }
}