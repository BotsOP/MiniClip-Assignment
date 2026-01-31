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
            Debug.Log($"entered spawnState");
            animator.CrossFade(SpawnAnimation, CROSS_FADE_DURATION);
        }
    }
}
