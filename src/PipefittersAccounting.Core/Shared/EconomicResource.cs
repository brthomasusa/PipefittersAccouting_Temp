#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Shared
{
    public class EconomicResource : Entity<Guid>
    {
        protected EconomicResource() { }

        public EconomicResource(EntityGuidID id, ResourceTypeEnum resourceType)
            : this()
        {
            Id = id;
            ResourceType = resourceType;
        }

        public ResourceTypeEnum ResourceType { get; protected set; }
    }

    public enum ResourceTypeEnum : int
    {
        Cash = 1,
        Inventory = 2,
        Product = 3,
        Labor = 4,
    }
}