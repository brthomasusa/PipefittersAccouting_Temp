using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IFinancierQueryService
    {
        Task<OperationResult<FinancierDetail>> GetFinancierDetails(GetFinancier queryParameters);
        Task<OperationResult<PagedList<FinancierListItems>>> GetFinancierListItems(GetFinanciers queryParameters);
        Task<OperationResult<PagedList<FinancierListItems>>> GetFinancierListItems(GetFinanciersByName queryParameters);
        Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters);
    }
}