using System;
using Components.ObjectPool;
using Components.UI;

namespace Components.Entities.Enemies.Moles
{
    public interface IMoleFactory
    {
        int GetDifficulty();
        float GetSpawnChance();
        void SetScaledDifficulty(float scaledSpawnChance);
        MoleViewer GetMoleViewer();
        Mole Create(Action<Entity> enemyDiedCallback, IObjectPoolManager objectPool, ScoreModel scoreModel, LevelModel levelModel);
    }
}
