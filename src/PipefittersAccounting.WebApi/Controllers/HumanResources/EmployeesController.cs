using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;

namespace PipefittersAccounting.WebApi.Controllers.HumanResources
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeAggregateQueryHandler _qryHdlr;
        private readonly IEmployeeAggregateCommandHandler _cmdHdlr;

        public EmployeesController
        (
            ILogger<EmployeesController> logger,
            IEmployeeAggregateQueryHandler queryHandler,
            IEmployeeAggregateCommandHandler commandHandler
        )
        {
            _logger = logger;
            _qryHdlr = queryHandler;
            _cmdHdlr = commandHandler;

            _logger.LogInformation("Logging injected into EmployeesController");
            // _logger.LogDebug("Hello from Debug");
        }
    }
}