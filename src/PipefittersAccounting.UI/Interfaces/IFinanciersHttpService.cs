using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Interfaces
{
    public interface IFinanciersHttpService
    {
        Task<OperationResult<FinancierReadModel>> CreateFinancier(FinancierWriteModel writeModel);
        Task<OperationResult<bool>> EditFinancier(FinancierWriteModel writeModel);
        Task<OperationResult<bool>> DeleteFinancier(FinancierWriteModel writeModel);


        Task<OperationResult<FinancierReadModel>> GetFinancierDetails(GetFinancier queryParameters);
        Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciers queryParameters);
        Task<OperationResult<PagingResponse<FinancierListItems>>> GetFinancierListItems(GetFinanciersByName queryParameters);
        Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters);
    }
}