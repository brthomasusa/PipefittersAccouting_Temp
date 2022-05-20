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
    [Route("api/{v:apiVersion}/CashAccounts")]
    public class CashAccountTransactionsController : Controller
    {
        private readonly ILogger<CashAccountTransactionsController> _logger;
        private readonly ICashAccountQueryService _qrySvc;
        private readonly ICashAccountApplicationService _appSvc;

        public CashAccountTransactionsController
        (
            ILogger<CashAccountTransactionsController> logger,
            ICashAccountQueryService queryService,
            ICashAccountApplicationService applicationService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _appSvc = applicationService;
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

        [HttpPost("cashtransaction/createdeposit/debtissueproceeds")]
        public async Task<IActionResult> CreateDepositForDebtIssueProceeds([FromBody] CreateCashAccountTransactionInfo writeModel)
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
                    return StatusCode(201, "Create cash deposit for debt issue proceeds succeeded; unable to return newly created cash transaction.");
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

        [HttpPost("cashtransaction/createdisbursement/loaninstallmentpayment")]
        public async Task<IActionResult> CreateDisbursementForLoanInstallmentPayment([FromBody] CreateCashAccountTransactionInfo writeModel)
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
                    return StatusCode(201, "Create cash disbursement for loan installment payment succeeded; unable to return newly created cash transaction.");
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

    }
}