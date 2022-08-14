using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class LoanAgreementRepository : ILoanAgreementRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public LoanAgreementRepository(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public Task<OperationResult<LoanAgreementDetail>> CreateLoanAgreement(LoanAgreementWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> EditLoanAgreement(LoanAgreementWriteModel writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> DeleteLoanAgreement(LoanAgreementWriteModel writeModel)
            => throw new NotImplementedException();

        public async Task<OperationResult<LoanAgreementDetail>> GetLoanAgreementDetails(GetLoanAgreement queryParameters)
            => await GetLoanAgreementHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreements queryParameters)
            => await GetLoanAgreementsHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<PagingResponse<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreementByLoanNumber queryParameters)
            => await GetLoanAgreementsHttpClient.Query(queryParameters, _client, _options);

        public async Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters)
            => await GetFinanciersLookupHttpClient.Query(_client, _options);
    }
}