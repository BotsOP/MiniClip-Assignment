using UnityEngine;

namespace Components.Player
{
    public interface IHitResolver
    {
        void ResolveHit(ref HammerData hammerData);
    }
    
    public class BasicHitResolver : IHitResolver
    {
        public void ResolveHit(ref HammerData hammerData)
        {
        }
    }
    
    public abstract class HitUpgrade : IHitResolver
    {
        protected readonly IHitResolver inner;

        protected HitUpgrade(IHitResolver inner)
        {
            this.inner = inner;
        }

        public virtual void ResolveHit(ref HammerData hammerData)
        {
        }
    }
    
    public class DoubleScoreUpgrade : HitUpgrade
    {
        public DoubleScoreUpgrade(IHitResolver inner) : base(inner) {}

        public override void ResolveHit(ref HammerData hammerData)
        {
            hammerData.damageScaling += hammerData.damageScaling;
            inner.ResolveHit(ref hammerData);
        }
    }
}
