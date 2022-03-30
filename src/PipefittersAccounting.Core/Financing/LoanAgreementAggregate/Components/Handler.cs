#pragma warning disable CS8618

using PipefittersAccounting.Core.Interfaces;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public abstract class Handler<T> : IHandler<T>
    {

        private IHandler<T> _next;

        public virtual void Handle(T request)
        {
            _next?.Handle(request);
        }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            _next = next;
            return _next;
        }
    }
}