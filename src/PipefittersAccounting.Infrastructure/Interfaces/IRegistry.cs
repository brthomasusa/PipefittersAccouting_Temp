

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface IRegistry
    {
        void RegisterService(string name, object service);

        T GetService<T>(string name);
    }
}