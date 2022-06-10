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
    public class LoanAgreementsController : ControllerBase
    {
        private readonly ILogger<FinanciersController> _logger;
        private readonly ILoanAgreementQueryService _qrySvc;
        private readonly ILoanAgreementApplicationService _cmdSvc;

        public LoanAgreementsController
        (
            ILogger<FinanciersController> logger,
            ILoanAgreementQueryService queryService,
            ILoanAgreementApplicationService commandService
        )
        {
            _logger = logger;
            _qrySvc = queryService;
            _cmdSvc = commandService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<PagedList<LoanAgreementListItem>>> GetLoanAgreements([FromQuery] GetLoanAgreements qryParams)
        {
            OperationResult<PagedList<LoanAgreementListItem>> result = await _qrySvc.GetLoanAgreementListItems(qryParams);

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

        [HttpGet("detail/{loanId:Guid}")]
        public async Task<ActionResult<LoanAgreementDetail>> GetLoanAgreementDetail(Guid loanId)
        {
            GetLoanAgreement queryParams = new() { LoanId = loanId };

            OperationResult<LoanAgreementDetail> result = await _qrySvc.GetLoanAgreementDetails(queryParams);

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
        public async Task<IActionResult> CreateLoanAgreement([FromBody] LoanAgreementWriteModel writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.CreateLoanAgreement(writeModel);
            if (writeResult.Success)
            {
                GetLoanAgreement queryParams = new() { LoanId = writeModel.LoanId };
                OperationResult<LoanAgreementDetail> result = await _qrySvc.GetLoanAgreementDetails(queryParams);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetLoanAgreementDetail), new { loanId = writeModel.LoanId }, result.Result);
                }
                else
                {
                    return StatusCode(201, "Create loan agreement succeeded; unable to return newly created loan agreement.");
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

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLoanAgreement([FromBody] DeleteLoanAgreementInfo writeModel)
        {
            OperationResult<bool> writeResult = await _cmdSvc.DeleteLoanAgreement(writeModel);

            if (writeResult.Success)
            {
                return StatusCode(200, "Loan agreement successfully deleted.");
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