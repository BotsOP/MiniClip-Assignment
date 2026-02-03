using Components.Grid;
using Components.Managers;
using PrimeTween;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Player.Upgrades
{
    public class BasicHitResolver : IHitResolver
    {
        private IDamageManager damageManager;
        public BasicHitResolver(IDamageManager damageManager)
        {
            this.damageManager = damageManager;
        }
        public void ResolveHit(ref HammerData hammerData)
        {
            DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, Vector2Int.zero, hammerData.damage, DamageSource.Player);
            
            hammerData.baseHammerTransform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 90);
            hammerData.baseHammerTransform.position = hammerData.worldPos;
            hammerData.baseHammerTransform.localRotation = hammerData.baseHammerRotation;
            
            damageManager.DoDamage(damageInfo);
            Sequence.Create(1, CycleMode.Restart, Ease.OutSine)
                .Group(Tween.LocalRotation(hammerData.baseHammerTransform.GetChild(0), Quaternion.identity, 0.1f));
        }
    }
}
