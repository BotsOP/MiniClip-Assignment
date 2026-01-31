using System;

namespace Components.Entities.Enemies
{
    public abstract class Enemy : Entity
    {
        protected int score;
        public int Score => score;
        protected Enemy(Action<Entity> diedCallback) : base(diedCallback)
        {
        }
    }
}
