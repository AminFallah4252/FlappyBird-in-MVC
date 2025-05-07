using System;
using Interfaces;
using Models;
using Signals;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class HUDController : IHUDController, IInitializable, IDisposable
    {
        [Inject] private GameModel gameModel;
        [Inject] private SignalBus signalBus;
        [Inject] private HUDView.Factory hudViewFactory;
        [Inject(Id = "MainCanvas")] private Canvas canvas;

        private HUDView hudView;

        public void Initialize()
        {
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void OnGameStateChanged(GameStateChangedSignal state)
        {
            switch (state.NewState)
            {
                case GameState.Waiting:
                    break;
                case GameState.Playing:
                    CreateHUD();
                    break;
                case GameState.GameOver:
                    DestroyHUD();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DestroyHUD()
        {
            if (hudView != null)
                Object.Destroy(hudView.gameObject);
        }

        private void CreateHUD()
        {
            hudView = hudViewFactory.Create(new HUDView.DataModel
            {
                BestScore = gameModel.BestScore
            });

            hudView.transform.SetParent(canvas.transform, false);
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
    }
}