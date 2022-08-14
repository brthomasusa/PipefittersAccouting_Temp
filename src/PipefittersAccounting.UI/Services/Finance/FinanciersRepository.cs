using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class FinanciersRepository : IFinanciersRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public FinanciersRepository(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<OperationResult<FinancierReadModel>> CreateFinancier(FinancierWriteModel writeModel)
            => await CreateFinancierHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> EditFinancier(FinancierWriteModel writeModel)
            => await EditFinancierHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> DeleteFinancier(FinancierWriteModel writeModel)
            => await DeleteFinancierHttpClient.Execute(writeModel, _client, _options);


        public async Task<OperationResult<FinancierReadModel>> GetFinancierDetails(GetFinancier queryParameters)
            => await GetFinancierHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciers queryParameters)
            => await GetFinanciersHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciersByName queryParameters)
            => await GetFinanciersHttpClient.Query(queryParameters, _client, _options);
    }
}
