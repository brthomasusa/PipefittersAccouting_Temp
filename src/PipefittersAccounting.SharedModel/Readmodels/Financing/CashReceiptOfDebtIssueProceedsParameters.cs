using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class CashReceiptOfDebtIssueProceedsParameters
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
    }
}