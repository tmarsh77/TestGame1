using Quantum;
using UnityEngine.Scripting;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class SpawnPlayerSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var config = f.FindAsset(f.RuntimeConfig.GameConfig);

            RuntimePlayer data = f.GetPlayerData(player);
            var entityPrototypeAsset = f.FindAsset(data.PlayerAvatar);
            var playerEntity = f.Create(entityPrototypeAsset);
            f.Add(playerEntity,
                new PlayerComponent
                {
                    PlayerRef = player,
                    MoveSpeed = config.Speed.Value,
                    DamagePerSecond = config.DamagePerSecond.Value,
                    AttackRadius = config.AttackRadius.Value,
                });
            f.Events.PlayerSpawned(playerEntity);
        }
    }
}