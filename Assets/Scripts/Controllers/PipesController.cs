using System;
using System.Collections.Generic;
using DG.Tweening;
using Interfaces;
using Models;
using Signals;
using UnityEngine;
using Views;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class PipesController : IPipesController, ITickable, IInitializable, IDisposable
    {
        [Inject] private SignalBus signalBus;
        [Inject] private PipesModel model;
        [Inject] private PipesView.Factory pipesViewFactory;
        [Inject(Id = "PipesParent")] private Transform pipesParent;
        [Inject(Id = "Ground")] private Transform ground;

        private GameState gameState;
        private Dictionary<PipesView, Tween> pipes = new();
        private float timer;

        public void Initialize()
        {
            signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void OnGameStateChanged(GameStateChangedSignal signal)
        {
            gameState = signal.NewState;
            if (gameState == GameState.GameOver)
            {
                KillTweens();
            }

            if (gameState != GameState.Playing) return;
            ClearPipes();
            SpawnPipe();
        }

        private void KillTweens()
        {
            foreach (var tween in pipes.Values)
            {
                tween.Kill();
            }
        }

        private void ClearPipes()
        {
            timer = 0;
            foreach (var (pipe, tween) in pipes)
            {
                tween.Kill();
                Object.DestroyImmediate(pipe.gameObject);
            }

            pipes.Clear();
        }

        public void SpawnPipe()
        {
            var tempPipe = pipesViewFactory.Create();
            tempPipe.transform.SetParent(pipesParent, false);
            tempPipe.transform.localPosition = new Vector3(0, model.GetRandomYPosition());

            ground.SetAsLastSibling();

            var tween = DOVirtual.DelayedCall(10, () =>
            {
                pipes.Remove(tempPipe);
                Object.Destroy(tempPipe.gameObject);
            });
            pipes.Add(tempPipe, tween);
        }

        public void MovePipes()
        {
            if (pipes.Count == 0)
                return;

            foreach (var pipe in pipes.Keys)
            {
                pipe.transform.position += model.GetPipeNextPosition();
            }
        }

        public void Tick()
        {
            if (gameState != GameState.Playing)
                return;
            MovePipes();

            if (timer > model.GetPipesInterval())
            {
                SpawnPipe();
                timer = 0;
            }

            timer += Time.deltaTime;
        }

        public void Dispose()
        {
            signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
    }
}