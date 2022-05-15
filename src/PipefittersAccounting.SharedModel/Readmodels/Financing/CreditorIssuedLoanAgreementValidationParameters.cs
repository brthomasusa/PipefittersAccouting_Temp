using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class CreditorIssuedLoanAgreementValidationParameters
    {
        public Guid LoanId { get; set; }
        public Guid FinancierId { get; set; }
    }
}