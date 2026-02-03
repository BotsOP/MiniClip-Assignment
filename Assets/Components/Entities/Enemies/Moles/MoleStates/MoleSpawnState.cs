using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public class MoleSpawnState : MoleBaseState
    {
        public MoleSpawnState(Animator animator) : base(animator)
        {
        }
        public override void OnEnter()
        {
            animator.CrossFade(SpawnAnimation, CROSS_FADE_DURATION);
        }
    }
}
