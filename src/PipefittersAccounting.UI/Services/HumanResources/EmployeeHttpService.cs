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
        public Task<OperationResult<bool>> CreateEmployeeInfo(EmployeeWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> CreateTimeCardInfo(TimeCardWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> EditTimeCardInfo(TimeCardWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> DeleteTimeCardInfo(TimeCardWriteModel writeModel)
            => throw new NotImplementedException();

        // Query system state with ReadModels        
        public Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters)
            => throw new NotImplementedException();

        public async Task<OperationResult<PagingResponse<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters)
            => await GetEmployeeListItemsHttpClient.Query(queryParameters, _client, _options);
        public Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagersParameters queryParameters)
            => throw new NotImplementedException();
        public Task<OperationResult<TimeCardDetail>> GetEmployeeTimeCardDetails(GetTimeCardParameter queryParameters)
            => throw new NotImplementedException();

        public Task<OperationResult<List<TimeCardListItem>>> GetEmployeeTimeCardListItems(GetEmployeeParameter queryParameters)
            => throw new NotImplementedException();

        public async Task<OperationResult<List<PayrollRegister>>> GetPayrollRegister(GetPayrollRegisterParameter queryParameters)
            => await GetPayrollRegisterHttpClient.Query(queryParameters, _client, _options);
    }
}