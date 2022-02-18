#pragma warning disable CS8604

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.Sqlite.QueryHandling.HumanResources
{
    public class GetEmployeeManagersQuery
    {
        public static async Task<OperationResult<List<EmployeeManager>>> Query(GetEmployeeManagers queryParameters, AppDbContext _dbContext)
        {
            try
            {
                var items = await (from emp in _dbContext.Employees.Where(s => s.IsSupervisor == true)
                                   select new EmployeeManager
                                   {
                                       ManagerId = emp.Id,
                                       ManagerFullName = $"{emp.EmployeeName.FirstName} {emp.EmployeeName.MiddleInitial} {emp.EmployeeName.LastName}"
                                   }).ToListAsync();

                return OperationResult<List<EmployeeManager>>.CreateSuccessResult(items);
            }
            catch (System.Exception ex)
            {
                return OperationResult<List<EmployeeManager>>.CreateFailure(ex);
            }
        }
    }
}