using UnityEngine;

namespace Components.StateMachine
{
    public class BaseState : IState
    {
        protected readonly Animator animator;

        protected BaseState(Animator animator)
        {
            this.animator = animator;
        }
        
        public void OnEnter()
        {
        }
        public void Update()
        {
        }
        public void FixedUpdate()
        {
        }
        public void OnExit()
        {
        }
    }
}
