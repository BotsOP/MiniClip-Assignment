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
            animator.CrossFade(IdleAnimation, CROSS_FADE_DURATION);
        }
    }

}
