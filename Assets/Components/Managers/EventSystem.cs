using System.Collections.Generic;

namespace Managers
{
    public enum EventType
    {
        SpawnRandomEnemy,
        AddScore,
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
        private static Dictionary<EventType, System.Action<T>> eventRegister = new Dictionary<EventType, System.Action<T>>();

        public static void Subscribe(EventType evt, System.Action<T> func)
        {
            eventRegister.TryAdd(evt, null);

            eventRegister[evt] += func;
        }

        public static void UnSubscribe(EventType evt, System.Action<T> func)
        {
            if (eventRegister.ContainsKey(evt))
            {
                eventRegister[evt] -= func;
            }
        }

        public static void RaiseEvent(EventType evt, T arg)
        {
            eventRegister[evt]?.Invoke(arg);
        }
    }
    
    public static class EventSystem<T, TA>
    {
        private static Dictionary<EventType, System.Action<T, TA>> eventRegister = new Dictionary<EventType, System.Action<T, TA>>();

        public static void Subscribe(EventType evt, System.Action<T, TA> func)
        {
            eventRegister.TryAdd(evt, null);

            eventRegister[evt] += func;
        }

        public static void UnSubscribe(EventType evt, System.Action<T, TA> func)
        {
            if (eventRegister.ContainsKey(evt))
            {
                eventRegister[evt] -= func;
            }
        }

        public static void RaiseEvent(EventType evt, T arg1, TA arg2)
        {
            eventRegister[evt]?.Invoke(arg1, arg2);
        }
    }
}