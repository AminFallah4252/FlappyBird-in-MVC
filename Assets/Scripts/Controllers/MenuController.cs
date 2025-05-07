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
    public class MenuController : IMenuController, IInitializable
    {
        [Inject] private GameModel model;
        [Inject] private MenuView.GameOverPanelFactory gameOverPanelFactory;
        [Inject] private MenuView.StartPanelFactory startPanelFactory;
        [Inject] private SignalBus signalBus;
        [Inject(Id = "MainCanvas")] private Canvas canvas;

        private MenuView menuView;

        public void Initialize()
        {
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void OnGameStateChanged(GameStateChangedSignal signal)
        {
            switch (signal.NewState)
            {
                case GameState.Waiting:
                    CreateMenuView();
                    break;
                case GameState.Playing:
                    DestroyMenuView();
                    break;
                case GameState.GameOver:
                    CreateGameOverPanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateGameOverPanel()
        {
            if (menuView != null)
                return;
            menuView = gameOverPanelFactory.Create(new MenuView.DataModel
            {
                Score = model.Score,
                BestScore = model.BestScore
            });
            menuView.transform.SetParent(canvas.transform, false);
            menuView.transform.SetAsFirstSibling();
        }

        private void DestroyMenuView()
        {
            Object.Destroy(menuView.gameObject);
        }

        private void CreateMenuView()
        {
            menuView = startPanelFactory.Create();
            menuView.transform.SetParent(canvas.transform, false);
            menuView.transform.SetAsFirstSibling();
        }
    }
}