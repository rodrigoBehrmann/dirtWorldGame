using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<T>(Action<T> listener) where T : Event
    {
        Type eventType = typeof(T);
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Delegate>();
        }

        eventListeners[eventType].Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : Event
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType].RemoveAll(e => e.Equals(listener));
        }
    }

    public static void Invoke<T>(T eventToPublish) where T : Event
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            foreach (var listener in eventListeners[eventType])
            {
                ((Action<T>)listener)(eventToPublish);
            }
        }
    }
}
