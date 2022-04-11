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
        private readonly ILoanAgreementCommandService _cmdSvc;

        public LoanAgreementsController
        (
            ILogger<FinanciersController> logger,
            ILoanAgreementQueryService queryService,
            ILoanAgreementCommandService commandService
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
        public async Task<ActionResult<LoanAgreementDetail>> GetFinancierDetail(Guid loanId)
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
    }
}