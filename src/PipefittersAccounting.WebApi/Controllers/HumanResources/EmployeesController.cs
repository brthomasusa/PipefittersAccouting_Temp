using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;
using PipefittersAccounting.WebApi.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.WebApi.Controllers.HumanResources
{
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeAggregateQueryService _qrySvc;
        private readonly IEmployeeAggregateCommandService _cmdSvc;

        public EmployeesController
        (
            ILogger<EmployeesController> logger,
            IEmployeeAggregateQueryService queryService,
            IEmployeeAggregateCommandService commandService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _cmdSvc = commandService;

            _logger.LogInformation("Logging injected into EmployeesController");
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<PagedList<EmployeeListItem>>> GetEmployees([FromQuery] GetEmployees getEmployeesParams)
        {
            OperationResult<PagedList<EmployeeListItem>> result = await _qrySvc.GetEmployeeListItems(getEmployeesParams);

            if (result.Success)
            {
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Result.MetaData));
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpGet]
        [Route("details/{employeeId:Guid}")]
        public async Task<ActionResult<EmployeeDetail>> Details(Guid employeeId)
        {
            GetEmployee queryParams =
            new GetEmployee
            {
                EmployeeID = employeeId
            };

            OperationResult<EmployeeDetail> result = await _qrySvc.GetEmployeeDetails(queryParams);

            if (result.Success)
            {
                return result.Result;
            }

            if (result.Exception is null)
            {
                _logger.LogWarning(result.NonSuccessMessage);
                return StatusCode(400, result.NonSuccessMessage);
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }


    }
}