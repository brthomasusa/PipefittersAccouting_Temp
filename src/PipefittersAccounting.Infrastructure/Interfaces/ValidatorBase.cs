using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public abstract class ValidatorBase<TWriteModel, TQueryServicesRegistry>
        where TWriteModel : IWriteModel
        where TQueryServicesRegistry : IQueryServicesRegistry
    {
        public ValidatorBase
        (
            TWriteModel writeModel,
            TQueryServicesRegistry queryServicesRegistry
        )
        {
            WriteModel = writeModel;
            QueryServicesRegistry = queryServicesRegistry;
        }

        protected TWriteModel WriteModel { get; init; }

        protected TQueryServicesRegistry QueryServicesRegistry { get; init; }

        public abstract Task<ValidationResult> Validate();
    }
}