namespace Components.Player.Upgrades
{
    public abstract class HitUpgrade : IHitResolver
    {
        private int level;
        protected readonly IHitResolver inner;
        
        public int Level => level;

        protected HitUpgrade(IHitResolver inner, int level)
        {
            this.inner = inner;
            this.level = level;
        }

        protected void ExtraHit(DamageInfo damageInfo)
        {
            
        }
        public void IncrementLevel() => level++;
        public virtual void ResolveHit(ref HammerData hammerData) {}
    }
}
