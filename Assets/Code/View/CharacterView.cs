using Quantum;
using UniRx;
using UnityEngine;

namespace Lk.TestGameMP.Code.View
{
    public class CharacterView : QuantumCallbacks
    {
        private QuantumEntityView _entityView;
        private Animator _animator;

        public readonly ReactiveProperty<int> Kills = new();
        public readonly ReactiveProperty<float> MoveSpeed = new();
        public readonly ReactiveProperty<float> AttackRadius = new();
        public readonly ReactiveProperty<float> AttackDamage = new();

        private void Awake()
        {
            _entityView = GetComponent<QuantumEntityView>();
            _animator = GetComponentInChildren<Animator>();
        }

        public override unsafe void OnUpdateView(QuantumGame game)
        {
            var frame = game.Frames.Predicted;
            var player = frame.Get<PlayerComponent>(_entityView.EntityRef);
            _animator.SetFloat("Velocity", player.Velocity.AsFloat);
            _animator.SetBool("IsAttacking", player.IsAttacking);

            var playerFilter = frame.Filter<PlayerComponent>();

            while (playerFilter.NextUnsafe(out _, out var playerComponent))
            {
                if (game.PlayerIsLocal(playerComponent->PlayerRef))
                {
                    Kills.Value = playerComponent->KilledEnemies;
                    MoveSpeed.Value = playerComponent->MoveSpeed.AsFloat;
                    AttackRadius.Value = playerComponent->AttackRadius.AsFloat;
                    AttackDamage.Value = playerComponent->DamagePerSecond.AsFloat;
                }
            }
        }
    }
}