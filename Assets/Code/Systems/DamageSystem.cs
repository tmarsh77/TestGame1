using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class DamageSystem : SystemMainThreadFilter<DamageSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PlayerComponent* Player;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.Player->IsAttacking = false;
            var gameConfig = f.FindAsset(f.RuntimeConfig.GameConfig);
            for (int i = 0; i < gameConfig.MaxTargets; ++i)
            {
                AttackClosest(f, ref filter);
            }
        }

        private void AttackClosest(Frame f, ref Filter filter)
        {
            var targetsFilter = f.Filter<Transform3D, HealthComponent>();

            HealthComponent* closestTarget = null;
            FP minDistance = filter.Player->AttackRadius;

            while (targetsFilter.NextUnsafe(out _, out var transform, out var healthComponent))
            {
                if (healthComponent->LastUpdateTick == f.Number) continue;

                FP dist = FPVector3.Distance(filter.Transform->Position, transform->Position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestTarget = healthComponent;
                }
            }

            if (closestTarget != null && closestTarget->Health > 0)
            {
                filter.Player->IsAttacking = true;
                closestTarget->Health -= filter.Player->DamagePerSecond * f.DeltaTime;
                if (closestTarget->Health <= 0)
                {
                    filter.Player->KilledEnemies++;
                }
                closestTarget->LastUpdateTick = f.Number;
            }
        }
    }
}