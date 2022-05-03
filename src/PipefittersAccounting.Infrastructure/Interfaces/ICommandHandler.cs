using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface ICommandHandler
    {
        Task<OperationResult<bool>> Handle(IWriteModel writeModel);
    }
}