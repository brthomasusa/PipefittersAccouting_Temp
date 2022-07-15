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
    public class FinanciersController : ControllerBase
    {
        private readonly ILogger<FinanciersController> _logger;
        private readonly IFinancierQueryService _qrySvc;
        private readonly IFinancierApplicationService _cmdSvc;

        public FinanciersController
        (
            ILogger<FinanciersController> logger,
            IFinancierQueryService queryService,
            IFinancierApplicationService commandService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _cmdSvc = commandService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<PagedList<FinancierListItems>>> GetFinanciers([FromQuery] GetFinanciers qryParams)
        {
            OperationResult<PagedList<FinancierListItems>> result = await _qrySvc.GetFinancierListItems(qryParams);

            if (result.Success)
            {
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Result.MetaData));
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedList<FinancierListItems>>> GetFinanciers([FromQuery] GetFinanciersByName qryParams)
        {
            OperationResult<PagedList<FinancierListItems>> result = await _qrySvc.GetFinancierListItems(qryParams);

            if (result.Success)
            {
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Result.MetaData));
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpGet("financierslookup")]
        public async Task<ActionResult<List<FinancierLookup>>> GetFinanciersLookup()
        {
            GetFinanciersLookup lookupParams = new() { };

            OperationResult<List<FinancierLookup>> result = await _qrySvc.GetFinanciersLookup(lookupParams);

            if (result.Success)
            {
                return result.Result;
            }

            _logger.LogError(result.Exception.Message);
            return StatusCode(500, result.Exception.Message);
        }

        [HttpGet("detail/{financierId:Guid}")]
        public async Task<ActionResult<FinancierDetail>> GetFinancierDetail(Guid financierId)
        {
            GetFinancier queryParams = new() { FinancierId = financierId };

            OperationResult<FinancierDetail> result = await _qrySvc.GetFinancierDetails(queryParams);

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
        public async Task<IActionResult> CreateFinancierInfo([FromBody] CreateFinancierInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.CreateFinancierInfo(writeModel);
            if (writeResult.Success)
            {
                GetFinancier queryParams = new() { FinancierId = writeModel.Id };
                OperationResult<FinancierDetail> queryResult = await _qrySvc.GetFinancierDetails(queryParams);

                if (queryResult.Success)
                {
                    return CreatedAtAction(nameof(GetFinancierDetail), new { financierId = writeModel.Id }, queryResult.Result);
                }
                else
                {
                    return StatusCode(201, "Create financier succeeded; unable to return newly created financier.");
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
        public async Task<IActionResult> EditFinancierInfo([FromBody] EditFinancierInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.EditFinancierInfo(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Financier info successfully updated.");
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
        public async Task<IActionResult> DeleteFinancierInfo([FromBody] DeleteFinancierInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.DeleteFinancierInfo(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Financier info successfully deleted.");
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