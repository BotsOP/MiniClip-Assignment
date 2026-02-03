using System;
using System.Collections.Generic;
using Components.Grid;
using Components.Managers;
using UnityEngine;

namespace Components.Player.Upgrades
{
    public class UpgradeManager : MonoBehaviour, IUpgradeManager, IDependencyProvider
    {
        public event Action<IHitResolver> UpdateHit;
        
        [SerializeField] private List<ScriptableObject> upgradeData;
        
        private readonly List<IHitUpgradeFactory> upgradeFactories = new List<IHitUpgradeFactory>();
        private List<IHitUpgradeFactory> activeUpgradeFactories;

        [Inject] private GridContext gridContext;
        [Inject] private IDamageManager damageManager;
        
        [Provide] private IUpgradeManager ProvideUpgradeManager()
        {
            return this;
        }

        private void OnValidate()
        {
            if (upgradeData == null)
                return;

            for (int i = upgradeData.Count - 1; i >= 0; i--)
            {
                if (!EnsureScriptableObjectIsUpgrade(upgradeData[i]))
                {
                    upgradeData.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Awake()
        {
            activeUpgradeFactories = new List<IHitUpgradeFactory>(upgradeFactories.Count);
            for (int i = 0; i < upgradeData.Count; i++)
            {
                if (!EnsureScriptableObjectIsUpgrade(upgradeData[i]))
                {
                    upgradeData.RemoveAt(i);
                    i--;
                    continue;
                }
                IHitUpgradeFactory hitUpgradeFactory = (IHitUpgradeFactory)upgradeData[i];
                hitUpgradeFactory.ResetLevel();
                upgradeFactories.Add(hitUpgradeFactory);
            }
        }
        private bool EnsureScriptableObjectIsUpgrade(ScriptableObject so)
        {
            if (so == null)
                return false;
                
            if (so is not IHitUpgradeFactory)
            {
                Debug.LogError($"{so.name} does not implement {nameof(IHitUpgradeFactory)} and was removed", this);
                return false;
            }
            
            return true;
        }

        public IHitUpgradeFactory[] TryGetRandomUpgrades(int amountRandomUpgrades)
        {
            int amountUpgrades = Mathf.Min(upgradeFactories.Count, amountRandomUpgrades);
            IHitUpgradeFactory[] possibleUpgradeFactories = new IHitUpgradeFactory[amountUpgrades];
            if (upgradeFactories.Count <= amountRandomUpgrades)
            {
                for (int i = 0; i < upgradeFactories.Count; i++)
                {
                    possibleUpgradeFactories[i] = upgradeFactories[i];
                }
                return possibleUpgradeFactories;
            }
            
            int[] distinctIndices = Helper.RandomDistinct(0, upgradeFactories.Count, amountRandomUpgrades);
            for (int i = 0; i < amountRandomUpgrades; i++)
            {
                int randomIndex = distinctIndices[i];
                possibleUpgradeFactories[i] = upgradeFactories[randomIndex];
            }
            return possibleUpgradeFactories;
        }

        public void AddUpgrade(IHitUpgradeFactory upgradeFactory)
        {
            upgradeFactory.IncreaseLevel();
            foreach (IHitUpgradeFactory activeUpgradeFactory in activeUpgradeFactories)
            {
                if (upgradeFactory != activeUpgradeFactory)
                    continue;
                RemakeUpgrades();
                return;
            }
            
            activeUpgradeFactories.Add(upgradeFactory);
            RemakeUpgrades();
        }

        private void RemakeUpgrades()
        {
            IHitResolver hit = new BasicHitResolver(damageManager);
            activeUpgradeFactories.Sort((a, b) => a.GetUpgradeOrder().CompareTo(b.GetUpgradeOrder()));
            foreach (IHitUpgradeFactory activeUpgradeFactory in activeUpgradeFactories)
            {
                hit = activeUpgradeFactory.Create(hit, gridContext, damageManager);
            }
            UpdateHit?.Invoke(hit);
        }
    }
}
