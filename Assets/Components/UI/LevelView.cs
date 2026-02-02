using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public interface ILevelView
    {
        void SetXp(float level);
    }

    public class LevelView : MonoBehaviour, ILevelView
    {
        [SerializeField] private Slider slider;
        
        public void SetXp(float level)
        {
            slider.value = level % 1.0f;
        }
    }
}
