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

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}