using System;
using Interfaces;
using Models;
using Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class BirdSpawner : IBirdSpawner, IInitializable, IDisposable
    {
        private readonly SignalBus signalBus;
        private readonly Transform background;
        private readonly BirdController.Factory birdControllerFactory;

        private IBirdController birdController;

        // Constructor injection with ID-based resolution
        public BirdSpawner(SignalBus signalBus, [Inject(Id = "Background")] Transform background, BirdController.Factory birdControllerFactory)
        {
            this.signalBus = signalBus;
            this.background = background;
            this.birdControllerFactory = birdControllerFactory;
        }

        public void Initialize()
        {
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void OnGameStateChanged(GameStateChangedSignal obj)
        {
            switch (obj.NewState)
            {
                case GameState.Playing:
                    Spawn();
                    break;
                case GameState.Waiting:
                case GameState.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Destroy()
        {
            if (birdController != null)
                Object.Destroy(birdController.GetGameObject());
        }

        public void Spawn()
        {
            Destroy();
            birdController = birdControllerFactory.Create();
            birdController.SetParent(background);
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
    }
}