namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public interface IHandler<T>
    {
        void Handle(T request);
        IHandler<T> SetNext(IHandler<T> next);
    }
}