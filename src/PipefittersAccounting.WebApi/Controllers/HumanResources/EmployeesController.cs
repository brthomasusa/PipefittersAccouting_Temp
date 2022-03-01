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
        }

        [HttpGet("list")]
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

        [HttpGet("managers")]
        public async Task<ActionResult<List<EmployeeManager>>> GetEmployeeManagers()
        {
            GetEmployeeManagers managersParams = new GetEmployeeManagers() { };

            OperationResult<List<EmployeeManager>> result = await _qrySvc.GetEmployeeManagers(managersParams);

            if (result.Success)
            {
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpGet("detail/{employeeId:Guid}", Name = "Details")]
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployeeInfo([FromBody] CreateEmployeeInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.CreateEmployeeInfo(writeModel);
            if (writeResult.Success)
            {
                GetEmployee queryParams = new GetEmployee { EmployeeID = writeModel.Id };
                OperationResult<EmployeeDetail> queryResult = await _qrySvc.GetEmployeeDetails(queryParams);

                if (queryResult.Success)
                {
                    return CreatedAtAction(nameof(Details), new { employeeId = writeModel.Id }, queryResult.Result);
                }
                else
                {
                    return StatusCode(201, "Create employee succeeded; unable to return newly created employee.");
                }
            }

            if (writeResult.Exception is null)
            {
                _logger.LogWarning(writeResult.NonSuccessMessage);
                return StatusCode(400, writeResult.NonSuccessMessage);
            }

            _logger.LogError(writeResult.Exception.Message);
            return StatusCode(500, writeResult.Exception.Message);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditEmployeeInfo([FromBody] EditEmployeeInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.EditEmployeeInfo(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Employee info successfully updated.");
            }

            if (writeResult.Exception is null)
            {
                _logger.LogWarning(writeResult.NonSuccessMessage);
                return StatusCode(400, writeResult.NonSuccessMessage);
            }

            _logger.LogError(writeResult.Exception.Message);
            return StatusCode(500, writeResult.Exception.Message);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployeeInfo([FromBody] DeleteEmployeeInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.DeleteEmployeeInfo(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Employee info successfully deleted.");
            }

            if (writeResult.Exception is null)
            {
                _logger.LogWarning(writeResult.NonSuccessMessage);
                return StatusCode(400, writeResult.NonSuccessMessage);
            }

            _logger.LogError(writeResult.Exception.Message);
            return StatusCode(500, writeResult.Exception.Message);
        }
    }
}