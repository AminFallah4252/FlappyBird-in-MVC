using System;
using System.Collections;
using DG.Tweening;
using Interfaces;
using Models;
using Signals;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class GameController : IGameController, IInitializable, IDisposable
    {
        private readonly GameModel model;
        private readonly SignalBus signalBus;

        public GameController(GameModel model, SignalBus signalBus)
        {
            this.model = model;
            this.signalBus = signalBus;
        }
        
        public void Initialize()
        {
            Time.timeScale = 0;

            model.CurrentState = GameState.Waiting;
            signalBus.Subscribe<StartGameRequestSignal>(StartGame);
            signalBus.Subscribe<GameOverSignal>(GameOver);
            signalBus.Subscribe<AddScoreSignal>(AddScore);

            DOVirtual.DelayedCall(0.1f,
                () => { signalBus.Fire(new GameStateChangedSignal { NewState = model.CurrentState }); });
        }

        public void StartGame()
        {
            Time.timeScale = 1;
            model.ResetScore();
            model.CurrentState = GameState.Playing;
            signalBus.Fire(new GameStateChangedSignal { NewState = model.CurrentState });
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            model.CurrentState = GameState.GameOver;
            signalBus.Fire(new GameStateChangedSignal { NewState = model.CurrentState });
        }

        public void AddScore()
        {
            model.AddScore();
            signalBus.Fire(new ScoreUpdatedSignal { Score = model.Score, BestScore = model.BestScore });
        }

        public void AddScore(int value)
        {
            model.AddScore(value);
            signalBus.Fire(new ScoreUpdatedSignal { Score = model.Score, BestScore = model.BestScore });
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<StartGameRequestSignal>(StartGame);
        }
    }
}