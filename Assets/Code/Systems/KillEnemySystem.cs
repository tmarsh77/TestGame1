using Quantum;
using UnityEngine.Scripting;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class KillEnemySystem : SystemMainThreadFilter<KillEnemySystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public HealthComponent* HealthComponent;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.HealthComponent->Health <= 0)
            {
                f.Destroy(filter.Entity);
                f.Signals.EnemyKilled();
            }
        }
    }
}