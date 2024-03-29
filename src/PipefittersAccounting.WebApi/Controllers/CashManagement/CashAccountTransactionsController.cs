using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.WebApi.Controllers.CashManagement
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/CashAccounts")]
    public class CashAccountTransactionsController : Controller
    {
        private readonly ILogger<CashAccountTransactionsController> _logger;
        private readonly ICashAccountQueryService _qrySvc;
        private readonly ICashAccountApplicationService _appSvc;
        private readonly IEmployeePayrollService _payrollService;

        public CashAccountTransactionsController
        (
            ILogger<CashAccountTransactionsController> logger,
            ICashAccountQueryService queryService,
            ICashAccountApplicationService applicationService,
            IEmployeePayrollService payrollService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _appSvc = applicationService;
            _payrollService = payrollService;
        }

        [HttpGet]
        [Route("cashtransaction/{cashTransactionId:int}")]
        public async Task<ActionResult<CashAccountTransactionDetail>> GetCashAccountTransactionDetails(int cashTransactionId)
        {
            GetCashAccountTransactionDetailParameters queryParams =
                new()
                {
                    CashTransactionId = cashTransactionId
                };

            OperationResult<CashAccountTransactionDetail> result = await _qrySvc.GetCashAccountTransactionDetail(queryParams);

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
        [Route("cashtransactions")]
        public async Task<ActionResult<PagedList<CashAccountTransactionListItem>>> GetCashAccountTransactionListItems([FromQuery] GetCashAccountTransactionListItemsParameters qryParams)
        {
            OperationResult<PagedList<CashAccountTransactionListItem>> result =
                await _qrySvc.GetCashAccountTransactionListItem(qryParams);

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

        [HttpPost("cashtransaction/createdeposit")]
        public async Task<IActionResult> CreateDeposit([FromBody] CashTransactionWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateCashDeposit(writeModel);

            if (writeResult.Success)
            {
                GetCashAccountTransactionDetailParameters queryParams =
                    new()
                    {
                        CashTransactionId = writeModel.CashTransactionId
                    };
                OperationResult<CashAccountTransactionDetail> getResult = await _qrySvc.GetCashAccountTransactionDetail(queryParams);

                if (getResult.Success)
                {
                    return CreatedAtAction(nameof(GetCashAccountTransactionDetails), new { cashTransactionId = writeModel.CashTransactionId }, getResult.Result);
                }
                else
                {
                    return StatusCode(201, "Create cash deposit succeeded; unable to return newly created cash transaction.");
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

        [HttpPost("cashtransaction/createdisbursement")]
        public async Task<IActionResult> CreateDisbursement([FromBody] CashTransactionWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateCashDisbursement(writeModel);

            if (writeResult.Success)
            {
                GetCashAccountTransactionDetailParameters queryParams =
                    new()
                    {
                        CashTransactionId = writeModel.CashTransactionId
                    };
                OperationResult<CashAccountTransactionDetail> getResult = await _qrySvc.GetCashAccountTransactionDetail(queryParams);

                if (getResult.Success)
                {
                    return CreatedAtAction(nameof(GetCashAccountTransactionDetails), new { cashTransactionId = writeModel.CashTransactionId }, getResult.Result);
                }
                else
                {
                    return StatusCode(201, "Create cash disbursement succeeded; unable to return newly created cash transaction.");
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

        [HttpPost("cashtransaction/createtransfer")]
        public async Task<IActionResult> CreateCashTransfer([FromBody] CashAccountTransferWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _appSvc.CreateCashTransfer(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(201, "Create cash transfer succeeded.");
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