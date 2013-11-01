using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Domain.Events
{
    public static class DomainEvents
    {
        public static IDisposable Register<T>(Action<T> callback) where T : IEvent
        {
            if (_registeredHandlers == null)
                _registeredHandlers = new List<Delegate>();

            _registeredHandlers.Add(callback);

            return new EventHandlerRegistration<T>(callback);
        }

        internal static void Unregister<T>(Action<T> callback) where T : IEvent
        {
            if (_registeredHandlers != null)
                _registeredHandlers.Remove(callback);
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (_registeredHandlers != null)
            {
                foreach (var handler in _registeredHandlers)
                {
                    if (handler is Action<T>)
                    {
                        try
                        {
                            ((Action<T>)handler)(args);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                }
            }
        }

        public static void FailWith<T>(T args) where T : IFailureEvent
        {
            if (_registeredHandlers != null)
            {
                foreach (var handler in _registeredHandlers)
                {
                    if (handler is Action<T>)
                    {
                        try
                        {
                            ((Action<T>)handler)(args);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                }
            }
        }

        public static void ClearCallbacks()
        {
            _registeredHandlers = null;
        }

        #region private

        [ThreadStatic]
        private static List<Delegate> _registeredHandlers;

        #endregion
    }
}
