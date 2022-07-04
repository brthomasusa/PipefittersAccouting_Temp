using Microsoft.AspNetCore.Mvc;

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.WebApi.Controllers.HumanResources
{
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/Employees")]
    [ApiController]
    public class TimeCardsController : ControllerBase
    {
        private readonly ILogger<TimeCardsController> _logger;
        private readonly IEmployeeAggregateQueryService _qrySvc;
        private readonly IEmployeeAggregateApplicationService _appSvc;

        public TimeCardsController
        (
            ILogger<TimeCardsController> logger,
            IEmployeeAggregateQueryService queryService,
            IEmployeeAggregateApplicationService applicationService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _appSvc = applicationService;
        }

        [HttpGet("timecards/{employeeId:Guid}")]
        public async Task<ActionResult<List<TimeCardListItem>>> GetTimeCards(Guid employeeId)
        {
            GetEmployeeParameter getEmployeesParams = new() { EmployeeID = employeeId };

            OperationResult<List<TimeCardListItem>> result = await _qrySvc.GetEmployeeTimeCardListItems(getEmployeesParams);

            if (result.Success)
            {
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpGet("timecards/payrollregister")]
        public async Task<ActionResult<List<PayrollRegister>>> GetPayrollRegister([FromQuery] GetPayrollRegisterParameter queryParameters)
        {
            OperationResult<List<PayrollRegister>> result = await _qrySvc.GetPayrollRegister(queryParameters);

            if (result.Success)
            {
                return result.Result;
            }

            _logger.LogError(result.NonSuccessMessage);
            return StatusCode(500, result.NonSuccessMessage);
        }

        [HttpGet("timecard/{timeCardId:Guid}")]
        public async Task<ActionResult<TimeCardDetail>> GetTimeCard(Guid timeCardId)
        {
            GetTimeCardParameter queryParameter = new() { TimeCardId = timeCardId };

            OperationResult<TimeCardDetail> result = await _qrySvc.GetEmployeeTimeCardDetails(queryParameter);

            if (result.Success)
            {
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpPost("timecard/create")]
        public async Task<IActionResult> CreateTimeCard([FromBody] TimeCardWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateTimeCardInfo(writeModel);

            if (writeResult.Success)
            {
                GetTimeCardParameter queryParameter = new() { TimeCardId = writeModel.TimeCardId };

                OperationResult<TimeCardDetail> result = await _qrySvc.GetEmployeeTimeCardDetails(queryParameter);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetTimeCard), new { timeCardId = writeModel.TimeCardId }, result.Result);
                }
                else
                {
                    return StatusCode(201, "Create employee timecard succeeded; unable to return newly created timecard.");
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

        [HttpPut("timecard/edit")]
        public async Task<IActionResult> EditTimeCard([FromBody] TimeCardWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.EditTimeCardInfo(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Employee timecard successfully updated.");
            }

            if (writeResult.Exception is null)
            {
                _logger.LogWarning(writeResult.NonSuccessMessage);
                return StatusCode(400, writeResult.NonSuccessMessage);
            }

            _logger.LogError(writeResult.Exception.Message);
            return StatusCode(500, writeResult.Exception.Message);
        }

        [HttpDelete("timecard/delete")]
        public async Task<IActionResult> DeleteTimeCard([FromBody] TimeCardWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.DeleteTimeCardInfo(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Employee timecard successfully deleted.");
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