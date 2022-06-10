using PipefittersAccounting.Infrastructure.Interfaces;

namespace PipefittersAccounting.Infrastructure.Application.Services
{
    public class QueryServicesRegistry : IQueryServicesRegistry
    {
        private readonly Dictionary<string, object> _configuration = new();

        public void RegisterService(string serviceName, object service)
        {
            if (!_configuration.ContainsKey(serviceName))
                _configuration.Add(serviceName, service);
        }

        public T GetService<T>(string serviceName) => (T)_configuration[serviceName];
    }
}