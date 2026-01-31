using Managers;
using UnityEngine;
using EventType = Managers.EventType;

namespace Components.Entities.Enemies.Moles
{
    public class MoleLeftState : MoleBaseState
    {
        private readonly Mole mole;
        public MoleLeftState(Animator animator, Mole mole) : base(animator)
        {
            this.mole = mole;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered left");
            mole?.InvokeDiedCallback();
        }
    }
}
