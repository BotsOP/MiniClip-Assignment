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
        private readonly List<IHitUpgradeFactory> activeUpgradeFactories = new List<IHitUpgradeFactory>();
        private GridContext gridContext;

        [Inject]
        private void InjectGridContext(IGridManager gridManager)
        {
            gridContext = gridManager.GetGridContext();
        }
        
        [Provide]
        private IUpgradeManager ProvideUpgradeManager()
        {
            return this;
        }

        private void OnValidate()
        {
            if (upgradeData == null)
                return;

            for (int i = upgradeData.Count - 1; i >= 0; i--)
            {
                if (!EnsureScriptableObjectIsUpgrade(i))
                {
                    i--;
                }
            }
        }

        private void Awake()
        {
            for (int i = 0; i < upgradeData.Count; i++)
            {
                if (!EnsureScriptableObjectIsUpgrade(i))
                {
                    i--;
                    continue;
                }
                upgradeFactories[i] = (IHitUpgradeFactory)upgradeData[i];
            }
        }
        private bool EnsureScriptableObjectIsUpgrade(int i)
        {
            ScriptableObject so = upgradeData[i];
                
            if (so == null)
                return false;
                
            if (so is not IHitUpgradeFactory)
            {
                // Debug.LogError($"{so.name} does not implement {nameof(IHitUpgradeFactory)} and was removed", this);
                upgradeData.RemoveAt(i);
                return false;
            }
            return true;
        }

        public IHitUpgradeFactory[] TryGet3RandomUpgrades()
        {
            int amountUpgrades = Mathf.Min(upgradeFactories.Count, 3);
            IHitUpgradeFactory[] possibleUpgradeFactories = new IHitUpgradeFactory[amountUpgrades];
            if (upgradeFactories.Count <= 3)
            {
                for (int i = 0; i < upgradeFactories.Count; i++)
                {
                    possibleUpgradeFactories[i] = upgradeFactories[i];
                }
                return possibleUpgradeFactories;
            }
            
            int[] distinctIndices = Helper.RandomDistinct(0, upgradeFactories.Count, 3);
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = distinctIndices[i];
                possibleUpgradeFactories[i] = upgradeFactories[randomIndex];
            }
            return possibleUpgradeFactories;
        }

        public void AddUpgrade(IHitUpgradeFactory upgradeFactory)
        {
            foreach (IHitUpgradeFactory activeUpgradeFactory in activeUpgradeFactories)
            {
                if (upgradeFactory != activeUpgradeFactory)
                    continue;
                activeUpgradeFactory.IncreaseLevel();
                RemakeUpgrades();
                return;
            }
            
            activeUpgradeFactories.Add(upgradeFactory);
            RemakeUpgrades();
        }

        private void RemakeUpgrades()
        {
            IHitResolver hit = new BasicHitResolver();
            activeUpgradeFactories.Sort((a, b) => a.GetUpgradeOrder().CompareTo(b.GetUpgradeOrder()));
            foreach (IHitUpgradeFactory activeUpgradeFactory in activeUpgradeFactories)
            {
                hit = activeUpgradeFactory.Create(hit, gridContext);
            }
            UpdateHit?.Invoke(hit);
        }
    }
}
