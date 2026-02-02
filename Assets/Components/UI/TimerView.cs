using TMPro;
using UnityEngine;

namespace Components.UI
{
    public interface ITimerView
    {
        void SetTimer(float time);
    }

    public class TimerView : MonoBehaviour, ITimerView
    {
        [SerializeField] private TMP_Text timerText;
        
        public void SetTimer(float time)
        {
            timerText.text = Mathf.FloorToInt(time).ToString();
        }
    }
}
