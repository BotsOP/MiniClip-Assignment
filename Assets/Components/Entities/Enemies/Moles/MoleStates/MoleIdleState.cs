using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public class MoleIdleState : MoleBaseState
    {
        public MoleIdleState(Animator animator) : base(animator)
        {
        }
        public override void OnEnter()
        {
            Debug.Log($"entered idleState");
            animator.CrossFade(IdleAnimation, CROSS_FADE_DURATION);
        }
    }

}
