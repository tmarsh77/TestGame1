using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Lk.TestGameMP
{
    public class TestGameMPInput : MonoBehaviour
    {
        private DispatcherSubscription _pollInputDispatcher;

        private void OnEnable()
        {
            _pollInputDispatcher = QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }

        private void OnDisable()
        {
            QuantumCallback.Unsubscribe(_pollInputDispatcher);
        }

        private bool _upgrade;

        public bool UpgradeInput
        {
            get
            {
                var tmp = _upgrade;
                _upgrade = false;
                return tmp;
            }
            set => _upgrade = value;
        }
        
        private void PollInput(CallbackPollInput callback)
        {
            Quantum.Input input = new Quantum.Input();

            input.Left = Input.GetKey(KeyCode.LeftArrow);
            input.Right = Input.GetKey(KeyCode.RightArrow);
            input.Up = Input.GetKey(KeyCode.UpArrow);
            input.Down = Input.GetKey(KeyCode.DownArrow);
            input.Upgrade = UpgradeInput;

            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }
    }
}