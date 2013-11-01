using System;

namespace Domain.Events
{
    public class EventHandlerRegistration<T> : IDisposable where T : IEvent
    {
        public EventHandlerRegistration(Action<T> callback)
        {
            _registeredHandler = callback;
        }

        void IDisposable.Dispose()
        {
            DomainEvents.Unregister(_registeredHandler);
            _registeredHandler = null;
        }

        private Action<T> _registeredHandler;
    }
}

