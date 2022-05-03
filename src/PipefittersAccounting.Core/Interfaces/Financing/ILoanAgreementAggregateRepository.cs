using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ILoanAgreementAggregateRepository : IAggregateRootRepository, IRepository<LoanAgreement>
    {

    }
}