using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateQueryService : IEmployeeAggregateQueryService
    {
        public Task<EmployeeDetail> Query(GetEmployee queryParameters)
        {
            throw new NotImplementedException();
        }
    }
}