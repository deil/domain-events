using System;

namespace Domain.Events
{
    /// <summary>
    /// Represents a handler for specified event type
    /// </summary>
    public interface IHandles<in T> where T : IEvent
    {
        void Handle(T e);
    }
}
