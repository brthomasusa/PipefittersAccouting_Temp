using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class StockSubscriptionRepository : IStockSubscriptionRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public StockSubscriptionRepository(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // Write
        public async Task<OperationResult<StockSubscriptionReadModel>> CreateStockSubscription(StockSubscriptionWriteModel writeModel)
            => await CreateStockSubscriptionHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> EditStockSubscription(StockSubscriptionWriteModel writeModel)
            => await EditStockSubscriptionHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> DeleteStockSubscription(StockSubscriptionWriteModel writeModel)
            => await DeleteStockSubscriptionHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<DividendDeclarationReadModel>> CreateDividendDeclaration(DividendDeclarationWriteModel writeModel)
            => await CreateDividendDeclarationHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> EditDividendDeclaration(DividendDeclarationWriteModel writeModel)
            => await EditDividendDeclarationHttpClient.Execute(writeModel, _client, _options);

        public async Task<OperationResult<bool>> DeleteDividendDeclaration(DividendDeclarationWriteModel writeModel)
            => await DeleteDividendDeclarationHttpClient.Execute(writeModel, _client, _options);


        // Read
        public async Task<OperationResult<StockSubscriptionReadModel>> GetStockSubscriptionReadModel(GetStockSubscriptionParameter queryParameters)
            => await GetStockSubscriptionHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItem queryParameters)
            => await GetStockSubscriptionsHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItemByInvestorName queryParameters)
            => await GetStockSubscriptionsHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<DividendDeclarationReadModel>> GetDividendDeclarationReadModel(GetDividendDeclarationParameter queryParameters)
            => await GetDividendDeclarationHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<DividendDeclarationListItem>>> GetDividendDeclarationListItems(GetDividendDeclarationsParameters queryParameters)
            => await GetDividendDeclarationsHttpClient.Query(queryParameters, _client, _options);
    }
}