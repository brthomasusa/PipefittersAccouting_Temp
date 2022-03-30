namespace PipefittersAccounting.Core.Interfaces
{
    public interface IHandler<T>
    {
        void Handle(T request);
        IHandler<T> SetNext(IHandler<T> next);
    }
}