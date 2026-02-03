using UnityEngine;
using EventType = Components.Managers.EventType;

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
            mole?.InvokeDiedCallback();
        }
    }
}
