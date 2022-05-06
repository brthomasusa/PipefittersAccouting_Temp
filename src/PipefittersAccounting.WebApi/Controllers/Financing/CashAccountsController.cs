using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.WebApi.Controllers.Financing
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]")]
    public class CashAccountsController : ControllerBase
    {
        private readonly ILogger<CashAccountsController> _logger;
        private readonly ICashAccountQueryService _qrySvc;
        private readonly ICashAccountApplicationService _appSvc;

        public CashAccountsController
        (
            ILogger<CashAccountsController> logger,
            ICashAccountQueryService queryService,
            ICashAccountApplicationService applicationService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _appSvc = applicationService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<PagedList<CashAccountListItem>>> GetCashAccounts([FromQuery] GetCashAccounts qryParams)
        {
            OperationResult<PagedList<CashAccountListItem>> result = await _qrySvc.GetCashAccountListItems(qryParams);

            if (result.Success)
            {
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Result.MetaData));
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

        [HttpGet("detail/{cashAccountId:Guid}")]
        public async Task<ActionResult<CashAccountDetail>> GetCashAccountDetail(Guid cashAccountId)
        {
            GetCashAccount queryParams = new() { CashAccountId = cashAccountId };

            OperationResult<CashAccountDetail> result = await _qrySvc.GetCashAccountDetails(queryParams);

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
        public async Task<IActionResult> CreateCashAccount([FromBody] CreateCashAccountInfo writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateCashAccount(writeModel);

            if (writeResult.Success)
            {
                GetCashAccount queryParams = new() { CashAccountId = writeModel.CashAccountId };
                OperationResult<CashAccountDetail> result = await _qrySvc.GetCashAccountDetails(queryParams);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetCashAccountDetail), new { cashAccountId = writeModel.CashAccountId }, result.Result);
                }
                else
                {
                    return StatusCode(201, "Create cash account succeeded; unable to return newly created cash account.");
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
        public async Task<IActionResult> EditCashAccountInfo([FromBody] EditCashAccountInfo writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.UpdateCashAccount(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Cash account info successfully updated.");
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
        public async Task<IActionResult> DeleteCashAccountInfo([FromBody] DeleteCashAccountInfo writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.DeleteCashAccount(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Cash account info successfully deleted.");
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