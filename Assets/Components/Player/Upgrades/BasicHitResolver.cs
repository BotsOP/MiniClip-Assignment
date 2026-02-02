using Components.Managers;
using PrimeTween;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Player.Upgrades
{
    public class BasicHitResolver : IHitResolver
    {
        public void ResolveHit(ref HammerData hammerData)
        {
            DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, Vector2Int.zero, hammerData.damage, DamageSource.Player);
            EventSystem<DamageInfo>.RaiseEvent(EventType.DoDamage, damageInfo);
            Sequence.Create(1, CycleMode.Restart, Ease.OutSine)
                .Group(Tween.LocalRotation(hammerData.pivot, Quaternion.identity, 0.1f));
        }
    }
}
