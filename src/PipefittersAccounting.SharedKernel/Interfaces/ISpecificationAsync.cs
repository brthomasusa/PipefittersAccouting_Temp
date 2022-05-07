using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.SharedKernel.Interfaces
{
    public interface ISpecificationAsync<T>
    {
        Task<bool> IsSatisfiedBy(T entity);
    }
}