using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


    }
}