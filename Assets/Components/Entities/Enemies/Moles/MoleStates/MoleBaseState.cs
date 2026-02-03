using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public abstract class MoleBaseState : IState
    {
        protected static readonly int SpawnAnimation = Animator.StringToHash("Underground to Idle");
        protected static readonly int IdleAnimation = Animator.StringToHash("Idle");
        protected static readonly int HitAnimation = Animator.StringToHash("Take Damage");
        protected static readonly int CastAnimation = Animator.StringToHash("Cast Spell");
        protected static readonly int LeaveAnimation = Animator.StringToHash("Idle to Underground");

        protected const float CROSS_FADE_DURATION = 0.1f;
        protected Animator animator;

        protected MoleBaseState(Animator animator)
        {
            this.animator = animator;
        }

        public virtual void OnEnter()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void FixedUpdate()
        {
        }
        public virtual void OnExit()
        {
        }
    }
}
