using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class SpawnEnemySystem : SystemSignalsOnly, ISignalEnemyKilled
    {
        public override void OnInit(Frame f)
        {
            var gameConfig = f.FindAsset(f.RuntimeConfig.GameConfig);
            for (int i = 0; i < gameConfig.InitialEnemyCount; ++i)
            {
                SpawnEnemy(f, gameConfig.EnemyPrototypes[f.RNG->Next(0, gameConfig.EnemyPrototypes.Length)]);
            }
        }

        private void SpawnEnemy(Frame f, AssetRef<EntityPrototype> enemyPrototype)
        {
            var gameConfig = f.FindAsset(f.RuntimeConfig.GameConfig);
            // spawn enemies in bounds of map with padding
            FP mapSize = gameConfig.MapSize - 1;
            EntityRef enemy = f.Create(enemyPrototype);
            Transform3D* enemyTransform = f.Unsafe.GetPointer<Transform3D>(enemy);
            enemyTransform->Position = new FPVector3(f.RNG->Next(-mapSize / 2, mapSize / 2), 0,
                f.RNG->Next(-mapSize / 2, mapSize / 2));
            enemyTransform->Rotation = FPQuaternion.Euler(0, f.RNG->Next(0, 360), 0);
        }

        public void EnemyKilled(Frame f)
        {
            var gameConfig = f.FindAsset(f.RuntimeConfig.GameConfig);
            SpawnEnemy(f, gameConfig.EnemyPrototypes[f.RNG->Next(0, gameConfig.EnemyPrototypes.Length)]);
        }
    }
}