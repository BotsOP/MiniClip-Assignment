using System.Collections.Generic;

namespace Components.Managers
{
    public enum EventType
    {
        DoDamage,
    }

    public static class EventSystem
    {
        private static Dictionary<EventType, System.Action> eventRegister = new();

        public static void Subscribe(EventType evt, System.Action func)
        {
            eventRegister.TryAdd(evt, null);

            eventRegister[evt] += func;
        }

        public static void UnSubscribe(EventType evt, System.Action func)
        {
            if (eventRegister.ContainsKey(evt))
            {
                eventRegister[evt] -= func;
            }
        }

        public static void RaiseEvent(EventType evt)
        {
            eventRegister[evt]?.Invoke();
        }
    }

    public static class EventSystem<T>
    {
        private readonly static Dictionary<EventType, System.Action<T>> EventRegister = new Dictionary<EventType, System.Action<T>>();

        public static void Subscribe(EventType evt, System.Action<T> func)
        {
            EventRegister.TryAdd(evt, null);

            EventRegister[evt] += func;
        }

        public static void UnSubscribe(EventType evt, System.Action<T> func)
        {
            if (EventRegister.ContainsKey(evt))
            {
                EventRegister[evt] -= func;
            }
        }

        public static void RaiseEvent(EventType evt, T arg)
        {
            EventRegister[evt]?.Invoke(arg);
        }
    }
}