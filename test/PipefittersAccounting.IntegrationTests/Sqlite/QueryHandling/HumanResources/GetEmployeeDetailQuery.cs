#pragma warning disable CS8604

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.Sqlite.QueryHandling.HumanResources
{
    public class GetEmployeeDetailQuery
    {
        public static async Task<OperationResult<EmployeeDetail>> Query(GetEmployee queryParameters, AppDbContext _dbContext)
        {
            int count = _dbContext.Employees.Where(e => e.Id == queryParameters.EmployeeID).Count();

            if (count == 0)
            {
                var error = new ArgumentException($"No employee with Id '{queryParameters.EmployeeID}' found.");
                return OperationResult<EmployeeDetail>.CreateFailure(error);
            }

            try
            {
                var details = (from emp in _dbContext.Employees.Where(e => e.Id == queryParameters.EmployeeID)
                               join supv in _dbContext.Employees.Where(s => s.IsSupervisor == true)
                               on emp.SupervisorId equals supv.Id
                               select new EmployeeDetail
                               {
                                   EmployeeId = emp.Id,
                                   SupervisorId = emp.SupervisorId,
                                   ManagerLastName = supv.EmployeeName.LastName,
                                   ManagerFirstName = supv.EmployeeName.FirstName,
                                   ManagerMiddleInitial = supv.EmployeeName.MiddleInitial,
                                   ManagerFullName = $"{supv.EmployeeName.FirstName} {supv.EmployeeName.MiddleInitial} {supv.EmployeeName.LastName}",
                                   LastName = emp.EmployeeName.LastName,
                                   FirstName = emp.EmployeeName.FirstName,
                                   MiddleInitial = emp.EmployeeName.MiddleInitial,
                                   EmployeeFullName = $"{emp.EmployeeName.FirstName} {emp.EmployeeName.MiddleInitial} {emp.EmployeeName.LastName}",
                                   SSN = emp.SSN,
                                   Telephone = emp.EmployeeTelephone,
                                   AddressLine1 = emp.EmployeeAddress.AddressLine1,
                                   AddressLine2 = emp.EmployeeAddress.AddressLine2,
                                   City = emp.EmployeeAddress.City,
                                   StateCode = emp.EmployeeAddress.StateCode,
                                   Zipcode = emp.EmployeeAddress.Zipcode,
                                   MaritalStatus = emp.MaritalStatus,
                                   Exemptions = emp.TaxExemptions,
                                   PayRate = emp.EmployeePayRate,
                                   StartDate = emp.EmploymentDate,
                                   IsActive = emp.IsActive,
                                   IsSupervisor = emp.IsSupervisor,
                                   CreatedDate = emp.CreatedDate
                               }).FirstOrDefaultAsync();

                return OperationResult<EmployeeDetail>.CreateSuccessResult(await details);
            }
            catch (Exception ex)
            {
                return OperationResult<EmployeeDetail>.CreateFailure(ex);
            }
        }
    }
}