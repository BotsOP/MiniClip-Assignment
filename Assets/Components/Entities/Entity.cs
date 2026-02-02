using System;

namespace Components.Entities
{
    public abstract class Entity
    {
        protected float health;
        protected Action<Entity> diedCallback;

        public float Health => health;
        public Action<Entity> DiedCallback => diedCallback;
        protected Entity(Action<Entity> diedCallback)
        {
            this.diedCallback += diedCallback;
        }

        public void AddDiedCallback(Action<Entity> diedCallback)
        {
            this.diedCallback += diedCallback;
        }

        public void InvokeDiedCallback()
        {
            diedCallback.Invoke(this);
        }

        public virtual void DoDamage(float damage)
        {
            health -= damage;
        }
    }
}
