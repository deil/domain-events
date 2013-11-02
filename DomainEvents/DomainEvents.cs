using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.ServiceLocation;

namespace Domain.Events
{
    /// <summary>
    /// Class for raising and handling events.
    /// </summary>
    public static class DomainEvents
    {
        /// <summary>
        /// Reference to service locator instance to query event handlers from.
        /// </summary>
        /// <value>Service locator instance</value>
        public static IServiceLocator ServiceLocator { get; set; }

        /// <summary>
        /// Registers callback on current thread for specified event type.
        /// </summary>
        /// <param name="callback">Callback</param>
        /// <typeparam name="T">Event type</typeparam>
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

        /// <summary>
        /// Raises an event on current thread.
        /// </summary>
        /// <param name="args">Event instance</param>
        /// <typeparam name="T">Event type</typeparam>
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            RaiseEvent(args, false);
        }

        public static void FailWith<T>(T args) where T : IFailureEvent
        {
            RaiseEvent(args, true);
        }

        /// <summary>
        /// Clears all registered callbacks for current thread.
        /// </summary>
        public static void ClearCallbacks()
        {
            _registeredHandlers = null;
        }

        #region private

        [ThreadStatic]
        private static List<Delegate> _registeredHandlers;

        private static void RaiseEvent<T>(T args, bool ignored) where T : IEvent
        {
            if (ServiceLocator != null)
            {
                foreach (var handler in ServiceLocator.GetAllInstances<IHandles<T>>())
                {
                    try
                    {
                        handler.Handle(args);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

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

        #endregion
    }
}
