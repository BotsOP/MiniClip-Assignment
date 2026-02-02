using System;

namespace Components.Entities.Enemies
{
    public abstract class Enemy : Entity
    {
        public int Score => score;
        public float Xp => xp;
        protected int score;
        protected float xp;
        protected Enemy(Action<Entity> diedCallback) : base(diedCallback)
        {
        }
    }
}
