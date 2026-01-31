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
            Debug.Log($"entered leave");
            animator.CrossFade(LeaveAnimation, 0);
        }
    }

}
