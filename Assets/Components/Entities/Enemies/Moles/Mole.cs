using System;
using Components.StateMachine;
using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public class Mole : Enemy
    {
        protected Animator animator;
        protected IMoleViewer moleViewer;
        protected StateMachine.StateMachine stateMachine;

        public IMoleViewer MoleViewer => moleViewer;

        public Mole(Action<Entity> diedCallback, MoleData moleData, IMoleViewer moleViewer) : base(diedCallback)
        {
            this.moleViewer = moleViewer;
            health = moleData.maxHealth;
            score = moleData.score;
            animator = moleViewer.GetAnimator();
            stateMachine = new StateMachine.StateMachine();
            
            MoleSpawnState spawnState = new MoleSpawnState(animator);
            MoleIdleState idleState = new MoleIdleState(animator);
            MoleLeaveState leaveState = new MoleLeaveState(animator);
            MoleDeathState deathState = new MoleDeathState(animator, this);
            MoleLeftState leftState = new MoleLeftState(animator, this);
            
            stateMachine.AddTransition(spawnState, idleState, new AnimationFinishedPredicate(animator));
            stateMachine.AddTransition(idleState, leaveState, new CountdownPredicate(moleData.leaveTime));
            stateMachine.AddTransition(leaveState, leftState, new AnimationFinishedPredicate(animator));
            stateMachine.AddAnyTransition(deathState, new FuncPredicate(HasDied));
            
            stateMachine.SetState(spawnState);
        }

        public void Update()
        {
            stateMachine.Update();
        }

        private bool HasDied()
        {
            return health <= 0;
        }
    }
}