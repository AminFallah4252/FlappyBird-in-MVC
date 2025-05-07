using System;
using Interfaces;
using Models;
using Signals;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class PlayerInputController : IPlayerInputController, IInitializable, ITickable
    {
        [Inject] private SignalBus signalBus;

        private GameState gameState;

        public void Initialize()
        {
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedSignal obj)
        {
            gameState = obj.NewState;
        }

        public void Tick()
        {
            switch (gameState)
            {
                case GameState.Waiting or GameState.GameOver:
                    if (Input.GetKeyDown(KeyCode.Space))
                        signalBus.Fire(new StartGameRequestSignal());
                    break;
                case GameState.Playing:
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    {
                        signalBus.Fire(new PlayerFlapSignal());
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}