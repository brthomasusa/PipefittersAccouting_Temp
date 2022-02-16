using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateQueryService
    {
        Task<EmployeeDetail> Query(GetEmployee queryParameters);
    }
}