using System;
using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    [CreateAssetMenu(fileName = "MoleData", menuName = "Enemies/Mole")]
    public class MoleData : ScriptableObject
    {
        [Header("Spawning")]
        public int difficulty;
        public float spawnChance;

        [Header("Mole Settings")] 
        public TypeOfMole typeOfMole;
        public MoleViewer moleInstance;
        public float leaveTime = 1;
        public float maxHealth = 10;
        public int score = 10;

        [HideInInspector] public float scaledSpawnChance;
    }
}
