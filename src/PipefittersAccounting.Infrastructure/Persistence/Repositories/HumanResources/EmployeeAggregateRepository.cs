#pragma warning disable CS8600

using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources
{
    public class EmployeeAggregateRepository : IEmployeeAggregateRepository, IDisposable
    {
        private bool isDisposed;
        private readonly AppDbContext _dbContext;

        public EmployeeAggregateRepository(AppDbContext ctx) => _dbContext = ctx;

        ~EmployeeAggregateRepository() => Dispose(false);

        public async Task<OperationResult<Employee>> GetByIdAsync(Guid id)
        {
            try
            {
                Employee employee = await _dbContext.Employees.FindAsync(id);

                if (employee is null)
                {
                    string msg = $"Unable to locate employee with id '{id}'.";
                    return OperationResult<Employee>.CreateFailure(msg);
                }

                return OperationResult<Employee>.CreateSuccessResult(employee);
            }
            catch (Exception ex)
            {
                return OperationResult<Employee>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> Exists(Guid id)
        {
            try
            {
                bool exist = await _dbContext.Employees.FindAsync(id) != null;
                if (exist)
                {
                    return OperationResult<bool>.CreateSuccessResult(exist);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure($"An employee with id '{id}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddAsync(Employee entity)
        {
            try
            {
                OperationResult<bool> result = await Exists(entity.Id);
                if (result.Success)
                {
                    string errMsg = $"Create employee failed! There is already an employee in the database with employee id '{entity.Id}'.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }
                else
                {
                    ExternalAgent agent = new(EntityGuidID.Create(entity.Id), AgentTypeEnum.Employee);
                    await _dbContext.ExternalAgents.AddAsync(agent);
                    await _dbContext.Employees.AddAsync(entity);
                    return OperationResult<bool>.CreateSuccessResult(true);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> Update(Employee entity)
        {
            try
            {
                _dbContext.Employees.Update(entity);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> Delete(Employee entity)
        {
            try
            {
                string errMsg = $"Delete employee failed, unable to locate external agent with id: {entity.Id}";
                ExternalAgent agent = _dbContext.ExternalAgents.Find(entity.Id) ?? throw new ArgumentNullException(errMsg);

                _dbContext.Employees.Remove(entity);
                _dbContext.ExternalAgents.Remove(agent);

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateEmployeeName(string lname, string fname, string mi)
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from emp in _dbContext.Employees.Where(e =>
                                        e.EmployeeName.LastName.ToUpper().Equals(lname.ToUpper()) &&
                                        e.EmployeeName.FirstName.ToUpper().Equals(fname.ToUpper()) &&
                                        e.EmployeeName.MiddleInitial.ToUpper().Equals(mi.ToUpper()))
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
                Guid returnValue = Guid.Empty;

                var details = await (from emp in _dbContext.Employees.Where(e => e.SSN == ssn).AsNoTracking()
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