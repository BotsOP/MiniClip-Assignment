using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public class MoleLeaveState : MoleBaseState
    {
        public MoleLeaveState(Animator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(LeaveAnimation, 0);
        }
    }

}
