#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8618
#pragma warning disable CS8625

using System.Reflection;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.SharedKernel
{
    public static class DomainEvent
    {
        [ThreadStatic]
        private static List<Delegate> _actions;

        public static IServiceProvider _serviceProvider { get; set; }


        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            _actions = _actions ?? new List<Delegate>();
            _actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            _actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (_serviceProvider != null)
            {
                //Fetch all handler of this type from the IoC container and invoke their handle method.
                foreach (var handler in (IEnumerable<IDomainEventHandler<T>>)_serviceProvider
                    .GetService(typeof(IEnumerable<IDomainEventHandler<T>>)))
                {
                    handler.Handle(args);
                }
            }

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }
}