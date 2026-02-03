using System;
using Components.StateMachine;
using Components.UI;
using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public class Mole : Enemy
    {
        public MoleViewer MoleViewer { get; private set; }
        public ScoreModel ScoreModel { get; private set; }
        public LevelModel LevelModel { get; private set; }

        protected StateMachine.StateMachine stateMachine;
        
        private bool hasDied;

        public Mole(Action<Entity> diedCallback, float maxHealth, int score, float xp, float leaveTime, MoleViewer moleViewer, ScoreModel scoreModel, LevelModel levelModel) : base(diedCallback)
        {
            SetupMole(maxHealth, score, xp, moleViewer, scoreModel, levelModel);
            Animator animator = moleViewer.GetAnimator();
            
            MoleSpawnState spawnState = new MoleSpawnState(animator);
            MoleIdleState idleState = new MoleIdleState(animator);
            MoleLeaveState leaveState = new MoleLeaveState(animator);
            MoleKilledState killedState = new MoleKilledState(animator, this);
            MoleLeftState leftState = new MoleLeftState(animator, this);
            stateMachine = new StateMachine.StateMachine();
            
            stateMachine.AddTransition(spawnState, idleState, new AnimationFinishedPredicate(animator));
            stateMachine.AddTransition(idleState, leaveState, new CountdownPredicate(leaveTime));
            stateMachine.AddTransition(leaveState, leftState, new AnimationFinishedPredicate(animator));
            stateMachine.AddAnyTransition(killedState, new FuncPredicate(HasDied));
            stateMachine.AddTransition(killedState, leaveState, new AnimationFinishedPredicate(animator));
            
            stateMachine.SetState(spawnState);
        }
        protected void SetupMole(float maxHealth, int score, float xp, MoleViewer moleViewer, ScoreModel scoreModel, LevelModel levelModel)
        {
            MoleViewer = moleViewer;
            ScoreModel = scoreModel;
            LevelModel = levelModel;
            
            health = maxHealth;
            this.score = score;
            this.xp = xp;
        }

        public virtual void Update()
        {
            stateMachine.Update();
        }

        protected virtual bool HasDied()
        {
            if (health <= 0 && !hasDied)
            {
                hasDied = true;
                return true;
            }
            return false;
        }
    }

}