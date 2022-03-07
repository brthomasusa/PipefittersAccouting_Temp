using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.SharedModel.Readmodels.HumanResources
{
    public class DoEmployeeDependencyCheck
    {
        public Guid EmployeeID { get; set; }
    }

    public class GetEmployee
    {
        public Guid EmployeeID { get; set; }
    }

    public class GetEmployees
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetEmployeeManagers
    {

    }
}