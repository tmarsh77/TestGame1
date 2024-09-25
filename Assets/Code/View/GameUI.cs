using Photon.Deterministic;
using Quantum;
using TMPro;
using UniRx;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Input = UnityEngine.Windows.Input;

namespace Lk.TestGameMP.Code.View
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text killsText;
        [SerializeField] private UISkillProgress speedSkillProgress;
        [SerializeField] private UISkillProgress attackRadiusSkillProgress;
        [SerializeField] private UISkillProgress damagePerSecondSkillProgress;
        [SerializeField] private Button upgradeButton;

        [SerializeField] private TestGameMPInput testGameMpInput;

        private QuantumEntityViewUpdater _viewUpdater;

        private DispatcherSubscription _playerSpawnedDispatcher;

        private void Awake()
        {
            _viewUpdater = FindObjectOfType<QuantumEntityViewUpdater>();
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }

        private void OnEnable()
        {
            _playerSpawnedDispatcher =
                QuantumEvent.Subscribe(listener: this, handler: (EventPlayerSpawned e) => OnGameStarted(e));
        }

        private void OnDisable()
        {
            QuantumEvent.Unsubscribe(_playerSpawnedDispatcher);
        }

        private void OnGameStarted(EventPlayerSpawned e)
        {
            var gameConfig = e.Game.Frames.Predicted.FindAsset(e.Game.Configurations.Runtime.GameConfig);
            if (_viewUpdater && _viewUpdater.GetView(e.Entity).GetComponent<CharacterView>() is { } view)
            {
                view.Kills.Subscribe(kills => killsText.text = $"Kills: {kills}").AddTo(this);
                view.MoveSpeed.Subscribe(speed => speedSkillProgress.SetValue(speed, gameConfig.Speed.MaxLevel.AsFloat))
                    .AddTo(this);
                view.AttackRadius.Subscribe(radius =>
                    attackRadiusSkillProgress.SetValue(radius, gameConfig.AttackRadius.MaxLevel.AsFloat)).AddTo(this);
                view.AttackDamage.Subscribe(damage =>
                        damagePerSecondSkillProgress.SetValue(damage, gameConfig.DamagePerSecond.MaxLevel.AsFloat))
                    .AddTo(this);
            }
        }

        private void OnUpgradeClicked()
        {
            testGameMpInput.UpgradeInput = true;
        }
    }
}