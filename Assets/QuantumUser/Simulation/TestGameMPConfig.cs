using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    [System.Serializable]
    public struct SkillInfo
    {
        public string Id;
        public FP Value;
        public FP MaxLevel;
        public FP UpgradeChance;
        public FP UpgradeAmount;
    }

    public class TestGameMPConfig : AssetObject
    {
        [Header("Player stats")]
        
        public SkillInfo Speed = new()
        {
            Value = 5,
            UpgradeChance = FP._0_10 + FP._0_05,
            MaxLevel = 20,
            UpgradeAmount = 1,
        };

        public SkillInfo AttackRadius = new()
        {
            Value = 3,
            UpgradeChance = FP._0_20,
            MaxLevel = 10,
            UpgradeAmount = 1,
        };

        public SkillInfo DamagePerSecond = new()
        {
            Value = 50,
            UpgradeChance = FP._0_20 + FP._0_05,
            MaxLevel = 150,
            UpgradeAmount = 1,
        };

        [Header("Enemies")] public AssetRef<EntityPrototype>[] EnemyPrototypes;

        public int InitialEnemyCount = 10;

        [Header("Map")] public FP MapSize = 20;

        [Header("Gameplay")] [Tooltip("Max targets that player can damage at the same time")]
        public int MaxTargets = 3;
    }
}