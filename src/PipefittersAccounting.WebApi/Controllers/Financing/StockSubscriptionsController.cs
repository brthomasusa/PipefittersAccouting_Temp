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
    [Route("api/{v:apiVersion}/StockSubscriptions")]
    public class StockSubscriptionsController : ControllerBase
    {
        private readonly ILogger<StockSubscriptionsController> _logger;
        private readonly IStockSubscriptionQueryService _qrySvc;
        private readonly IStockSubscriptionApplicationService _appSvc;

        public StockSubscriptionsController
        (
            ILogger<StockSubscriptionsController> logger,
            IStockSubscriptionQueryService queryService,
            IStockSubscriptionApplicationService applicationService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _appSvc = applicationService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptions([FromQuery] GetStockSubscriptionListItemParameters qryParams)
        {
            OperationResult<PagedList<StockSubscriptionListItem>> result = await _qrySvc.GetStockSubscriptionListItems(qryParams);

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

        [HttpGet("detail/{stockId:Guid}")]
        public async Task<ActionResult<StockSubscriptionDetails>> GetStockSubscriptionDetail(Guid stockId)
        {
            GetStockSubscriptionParameter queryParams = new() { StockId = stockId };

            OperationResult<StockSubscriptionDetails> result = await _qrySvc.GetStockSubscriptionDetails(queryParams);

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
        public async Task<IActionResult> CreateStockSubscription([FromBody] StockSubscriptionWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateStockSubscription(writeModel);

            if (writeResult.Success)
            {
                GetStockSubscriptionParameter queryParams = new() { StockId = writeModel.StockId };
                OperationResult<StockSubscriptionDetails> result = await _qrySvc.GetStockSubscriptionDetails(queryParams);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetStockSubscriptionDetail), new { stockId = writeModel.StockId }, result.Result);
                }
                else
                {
                    return StatusCode(201, "Create stock subscription succeeded; unable to return newly created stock subscription.");
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
        public async Task<IActionResult> EditStockSubscription([FromBody] StockSubscriptionWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.EditStockSubscription(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Stock subscription info successfully updated.");
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
        public async Task<IActionResult> DeleteStockSubscription([FromBody] StockSubscriptionWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.DeleteStockSubscription(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Stock subscription info successfully deleted.");
            }

            if (writeResult.Exception is null)
            {
                _logger.LogWarning(writeResult.NonSuccessMessage);
                return StatusCode(400, writeResult.NonSuccessMessage);
            }

            _logger.LogError(writeResult.Exception.Message);
            return StatusCode(500, writeResult.Exception.Message);
        }

        [HttpGet]
        [Route("dividenddeclaration/{dividendId:Guid}")]
        public async Task<ActionResult<DividendDeclarationDetails>> GetDividendDeclarationDetails(Guid dividendId)
        {
            GetDividendDeclarationParameter queryParams = new() { DividendId = dividendId };

            OperationResult<DividendDeclarationDetails> result = await _qrySvc.GetDividendDeclarationDetails(queryParams);

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

        [HttpGet]
        [Route("dividenddeclarations")]
        public async Task<ActionResult<PagedList<DividendDeclarationListItem>>> GetDividendDeclarationListItems([FromQuery] GetDividendDeclarationsParameters qryParams)
        {
            OperationResult<PagedList<DividendDeclarationListItem>> result =
                await _qrySvc.GetDividendDeclarationListItems(qryParams);

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

        [HttpPost("dividenddeclaration/create")]
        public async Task<IActionResult> CreateDividendDeclaration([FromBody] DividendDeclarationWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateDividendDeclaration(writeModel);

            if (writeResult.Success)
            {
                GetDividendDeclarationParameter queryParams = new() { DividendId = writeModel.DividendId };
                OperationResult<DividendDeclarationDetails> getResult = await _qrySvc.GetDividendDeclarationDetails(queryParams);

                if (getResult.Success)
                {
                    return CreatedAtAction(nameof(GetDividendDeclarationDetails), new { dividendId = writeModel.DividendId }, getResult.Result);
                }
                else
                {
                    return StatusCode(201, "Create dividend declaration succeeded; unable to return newly created declaration.");
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

        [HttpPut("dividenddeclaration/edit")]
        public async Task<IActionResult> EditDividendDeclaration([FromBody] DividendDeclarationWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.EditDividendDeclaration(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Dividend declaration info successfully updated.");
            }

            if (writeResult.Exception is null)
            {
                _logger.LogWarning(writeResult.NonSuccessMessage);
                return StatusCode(400, writeResult.NonSuccessMessage);
            }

            _logger.LogError(writeResult.Exception.Message);
            return StatusCode(500, writeResult.Exception.Message);
        }


        [HttpDelete("dividenddeclaration/delete")]
        public async Task<IActionResult> DeleteDividendDeclaration([FromBody] DividendDeclarationWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.DeleteDividendDeclaration(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Dividend declaration info successfully deleted.");
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