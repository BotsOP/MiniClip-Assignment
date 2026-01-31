using System;
using Managers;
using UnityEngine;
using EventType = Managers.EventType;

namespace Components.Entities.Enemies.Moles
{
    public class MoleDeathState : MoleBaseState
    {
        private Mole mole;
        public MoleDeathState(Animator animator, Mole mole) : base(animator)
        {
            this.mole = mole;
        }

        public override void OnEnter()
        {
            EventSystem<int>.RaiseEvent(EventType.AddScore, mole.Score);
            mole.InvokeDiedCallback();
            animator.CrossFade(HitAnimation, CROSS_FADE_DURATION);
        }
    }

}
