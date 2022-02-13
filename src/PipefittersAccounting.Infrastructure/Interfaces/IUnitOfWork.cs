namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}