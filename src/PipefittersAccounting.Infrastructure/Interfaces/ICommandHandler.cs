using System.Threading.Tasks;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface ICommandHandler
    {
        Task<OperationResult> Handle(IWriteModel writeModel);
    }
}