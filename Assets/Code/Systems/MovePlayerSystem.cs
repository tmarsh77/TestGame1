using Photon.Deterministic;
using Quantum;
using UnityEngine.Scripting;
using Input = Quantum.Input;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class MovePlayerSystem : SystemMainThreadFilter<MovePlayerSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PlayerComponent* Player;
        }

        private static readonly FP AccelerationMultiplier = 10;

        public override void Update(Frame f, ref Filter filter)
        {
            Input* input = default;
            if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerComponent* playerComponent))
            {
                input = f.GetPlayerInput(playerComponent->PlayerRef);
            }

            UpdateMovement(f, ref filter, input);
        }
        
        private void UpdateMovement(Frame f, ref Filter filter, Input* input)
        {
            var config = f.FindAsset(f.RuntimeConfig.GameConfig);

            FP TurnSpeed = 360;

            FPVector3 TargetPosition = filter.Transform->Position;

            FP acc = -f.DeltaTime;
            if (input->Up)
            {
                TargetPosition += filter.Transform->Forward * filter.Player->MoveSpeed * f.DeltaTime;
                acc = f.DeltaTime;
            }

            if (input->Down)
            {
                TargetPosition -= filter.Transform->Forward * filter.Player->MoveSpeed * f.DeltaTime;
                acc = f.DeltaTime;
            }

            filter.Player->Velocity = FPMath.Clamp01(filter.Player->Velocity + acc * AccelerationMultiplier);

            TargetPosition.X = FPMath.Clamp(TargetPosition.X, -config.MapSize / 2, config.MapSize / 2);
            TargetPosition.Z = FPMath.Clamp(TargetPosition.Z, -config.MapSize / 2, config.MapSize / 2);

            filter.Transform->Position = TargetPosition;

            if (input->Left)
            {
                filter.Transform->Rotation *= FPQuaternion.Euler(0, -TurnSpeed * f.DeltaTime, 0);
            }

            if (input->Right)
            {
                filter.Transform->Rotation *= FPQuaternion.Euler(0, TurnSpeed * f.DeltaTime, 0);
            }
        }
    }
}