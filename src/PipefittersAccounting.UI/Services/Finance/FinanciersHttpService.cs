using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class FinanciersHttpService : IFinanciersHttpService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public FinanciersHttpService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<OperationResult<FinancierReadModel>> CreateFinancier(FinancierWriteModel writeModel)
            => await CreateFinancierHttpClient.Create(writeModel, _client, _options);

        public Task<OperationResult<bool>> EditFinancier(FinancierWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> DeleteFinancier(FinancierWriteModel writeModel)
            => throw new NotImplementedException();


        public async Task<OperationResult<FinancierReadModel>> GetFinancierDetails(GetFinancier queryParameters)
            => await GetFinancierHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciers queryParameters)
            => await GetFinanciersHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciersByName queryParameters)
            => await GetFinanciersHttpClient.Query(queryParameters, _client, _options);

        public Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters)
            => throw new NotImplementedException();
    }
}
