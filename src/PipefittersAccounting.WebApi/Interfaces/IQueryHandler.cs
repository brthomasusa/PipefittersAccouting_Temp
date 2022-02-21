using System.Threading.Tasks;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.WebApi.Interfaces
{
    public interface IQueryHandler
    {
        Task<OperationResult<IReadModel>> Handle<TQueryParam>(TQueryParam queryParam);
    }
}