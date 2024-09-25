using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.Scripting;

namespace Lk.TestGameMP.Code.Systems
{
    [Preserve]
    public unsafe class UpdateCameraSystem : SystemMainThreadFilter<UpdateCameraSystem.Filter>
    {
        public unsafe struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public CameraMarkerComponent* Camera;
        }

        public override void Update(Frame f, ref UpdateCameraSystem.Filter filter)
        {
            var playerFilter = f.Filter<Transform3D, PlayerComponent>();
            while (playerFilter.NextUnsafe(out _, out var transform, out var _))
            {
                var targetPosition = transform->Position + new FPVector3(0, 10, -5);
                filter.Transform->Position = FPVector3.Lerp(filter.Transform->Position, targetPosition, f.DeltaTime);
            }
        }
    }
}