#pragma warning disable CS8618
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public abstract class Handler<T> : IHandler<T>
    {

        private IHandler<T> Next { get; set; }

        public virtual void Handle(T request)
        {
            Next?.Handle(request);
        }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            Next = next;
            return Next;
        }
    }
}