using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Interfaces
{
    public interface IFinanciersHttpService
    {
        Task<OperationResult<FinancierDetail>> GetFinancierDetails(GetFinancier queryParameters);
        Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciers queryParameters);
        Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters);
    }
}