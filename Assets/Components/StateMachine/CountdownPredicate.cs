using UnityEngine;

namespace Components.StateMachine
{
    public class CountdownPredicate : IPredicate
    {
        private readonly float countdown;
        private float delay;
        public CountdownPredicate(float countdown)
        {
            this.countdown = countdown;
        }
        public bool Evaluate()
        {
            return Time.timeSinceLevelLoad > delay;
        }
        public void OnEnter()
        {
            delay = countdown + Time.timeSinceLevelLoad;
        }
    }
}
