using System;

namespace Components.UI
{
    public class TimerModel
    {
        public event Action<float> TimeChanged;
        public float Time { get; private set; }

        public TimerModel(float initialTime)
        {
            Time = initialTime;
        }

        public void IncrementTime(float time)
        {
            Time += time;
            TimeChanged?.Invoke(Time);
        }
    }
}
