using System.Linq;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.Scripting;
using Input = Quantum.Input;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class UpgradeSystem : SystemMainThreadFilter<UpgradeSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PlayerComponent* Player;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            Input* input = default;
            if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerComponent* pc))
            {
                input = f.GetPlayerInput(pc->PlayerRef);
            }

            if (!input->Upgrade) return;

            var config = f.FindAsset(f.RuntimeConfig.GameConfig);

            var playerComponent = filter.Player;

            FP rand = f.RNG->Next(FP._0, FP._1);
            FP c = 0;
            foreach (var skill in new[]
                     {
                         config.Speed,
                         config.AttackRadius,
                         config.DamagePerSecond,
                     }.OrderBy(skillInfo => skillInfo.UpgradeChance))
            {
                c += skill.UpgradeChance;
                if (rand < c)
                {
                    switch (skill)
                    {
                        case { Id: var id } when id == config.Speed.Id:
                            playerComponent->MoveSpeed += config.Speed.UpgradeAmount;
                            break;
                        case { Id: var id } when id == config.AttackRadius.Id:
                            playerComponent->AttackRadius += config.AttackRadius.UpgradeAmount;
                            break;
                        case { Id: var id } when id == config.DamagePerSecond.Id:
                            playerComponent->DamagePerSecond += config.DamagePerSecond.UpgradeAmount;
                            break;
                    }

                    break;
                }
            }
        }
    }
}