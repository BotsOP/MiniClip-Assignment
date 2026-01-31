using UnityEngine;

namespace Components.StateMachine
{
    public class AnimationFinishedPredicate : IPredicate
    {
        private Animator animator;
        
        public AnimationFinishedPredicate(Animator animator)
        {
            this.animator = animator;
        }
        public bool Evaluate()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
        public void OnEnter()
        {
        }
    }

}
