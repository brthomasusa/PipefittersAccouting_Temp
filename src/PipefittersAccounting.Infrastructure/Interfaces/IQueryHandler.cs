using System.Threading.Tasks;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface IQueryHandler
    {
        Task<OperationResult<IReadModel>> Handle<TQueryParam>(TQueryParam queryParam);
    }
}