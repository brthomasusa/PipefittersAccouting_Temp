#pragma warning disable CS8604

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.Sqlite.QueryHandling.HumanResources
{
    public class GetEmployeeListItemsQuery
    {
        public static async Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployees queryParameters, AppDbContext _dbContext)
        {
            try
            {
                int count = _dbContext.Employees.Count();

                var items = await (from emp in _dbContext.Employees
                                   join supv in _dbContext.Employees.Where(s => s.IsSupervisor == true)
                                   on emp.SupervisorId equals supv.Id
                                   orderby emp.EmployeeName.LastName
                                   select new EmployeeListItem
                                   {
                                       EmployeeId = emp.Id,
                                       EmployeeFullName = $"{emp.EmployeeName.FirstName} {emp.EmployeeName.MiddleInitial} {emp.EmployeeName.LastName}",
                                       Telephone = emp.EmployeeTelephone,
                                       IsActive = emp.IsActive,
                                       IsSupervisor = emp.IsSupervisor,
                                       ManagerFullName = $"{supv.EmployeeName.FirstName} {supv.EmployeeName.MiddleInitial} {supv.EmployeeName.LastName}"
                                   })
                                     .Skip(queryParameters.Page - 1)
                                     .Take(queryParameters.PageSize)
                                     .ToListAsync();

                var pagedList = PagedList<EmployeeListItem>.CreatePagedList(items, count, queryParameters.Page, queryParameters.PageSize);

                return OperationResult<PagedList<EmployeeListItem>>.CreateSuccessResult(pagedList);
            }
            catch (System.Exception ex)
            {
                return OperationResult<PagedList<EmployeeListItem>>.CreateFailure(ex);
            }
        }
    }
}