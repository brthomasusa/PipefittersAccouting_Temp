using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.SharedKernel.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> GetByIdAsync(Guid id);
        Task<bool> Exists(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}