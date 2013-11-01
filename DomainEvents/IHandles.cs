using System;

namespace Domain.Events
{
    public interface IHandles<in T> where T : IEvent
    {
        void Handle(T e);
    }
}
