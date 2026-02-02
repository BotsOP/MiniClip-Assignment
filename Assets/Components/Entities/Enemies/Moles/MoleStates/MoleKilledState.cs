using System;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Entities.Enemies.Moles
{
    public class MoleKilledState : MoleBaseState
    {
        private Mole mole;
        public MoleKilledState(Animator animator, Mole mole) : base(animator)
        {
            this.mole = mole;
        }

        public override void OnEnter()
        {
            mole.ScoreModel.AddScore(mole.Score);
            mole.LevelModel.AddXp(mole.Xp);
        }
    }

}
