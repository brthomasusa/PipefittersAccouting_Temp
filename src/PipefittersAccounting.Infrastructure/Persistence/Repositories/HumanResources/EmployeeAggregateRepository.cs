#pragma warning disable CS8603

using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources
{
    public class EmployeeAggregateRepository : IEmployeeAggregateRepository, IDisposable
    {
        private bool isDisposed;
        private readonly AppDbContext _dbContext;

        public EmployeeAggregateRepository(AppDbContext ctx) => _dbContext = ctx;

        ~EmployeeAggregateRepository() => Dispose(false);

        public async Task<Employee> GetByIdAsync(Guid id) => await _dbContext.Employees.FindAsync(id);

        public async Task<bool> Exists(Guid id) => await _dbContext.Employees.FindAsync(id) != null;

        public async Task AddAsync(Employee entity) => await _dbContext.Employees.AddAsync(entity);

        public void Update(Employee entity) => _dbContext.Employees.Update(entity);

        public void Delete(Employee entity)
        {
            _dbContext.Employees.Remove(entity);
            _dbContext.ExternalAgents.Remove(entity.ExternalAgent);
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateEmployeeName(string lname, string fname, string mi)
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from emp in _dbContext.Employees.Where(e =>
                                        e.EmployeeName.LastName.Equals(lname) &&
                                        e.EmployeeName.FirstName.Equals(fname) &&
                                        e.EmployeeName.MiddleInitial.Equals(mi))
                                    .AsNoTracking()
                                     select new { EmployeeId = emp.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.EmployeeId;
                }

                return OperationResult<Guid>.CreateSuccessResult(returnValue);
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateSSN(string ssn)
        {
            try
            {
                Guid returnValue = Guid.NewGuid();

                var details = await (from emp in _dbContext.Employees.Where(e => e.SSN.Equals(ssn)).AsNoTracking()
                                     select new { EmployeeId = emp.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.EmployeeId;
                }

                return OperationResult<Guid>.CreateSuccessResult(returnValue);
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                // free managed resources
                _dbContext.Dispose();
            }

            isDisposed = true;
        }
    }
}