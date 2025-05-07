using Controllers;
using Models;
using NUnit.Framework;
using Signals;
using UnityEngine;
using Zenject;

namespace Tests.Editor
{
    [TestFixture]
    public class GameController_ZenjectTests : ZenjectUnitTestFixture
    {
        private GameController _controller;
        private GameModel _model;
        private SignalBus _signalBus;

        [SetUp]
        public void Setup()
        {
            // Given: GameModel and SignalBus are installed
            _model = ScriptableObject.CreateInstance<GameModel>();

            Container.BindInstance(_model);
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<StartGameRequestSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<AddScoreSignal>();
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<ScoreUpdatedSignal>();

            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();

            Container.Inject(this); // if you had fields to inject
            _controller = Container.Resolve<GameController>();
            _signalBus = Container.Resolve<SignalBus>();
        }

        [Test]
        public void Given_ControllerInitialized_When_InitializeCalled_Then_StateIsWaiting_And_TimeScaleIsZero()
        {
            // When
            _controller.Initialize();

            // Then
            Assert.AreEqual(GameState.Waiting, _model.CurrentState);
            Assert.AreEqual(0f, Time.timeScale);
        }

        [Test]
        public void Given_ControllerInitialized_When_StartGameCalled_Then_StateIsPlaying_And_ScoreIsReset()
        {
            // Given
            _controller.Initialize();
            _model.AddScore(10);

            // When
            _controller.StartGame();

            // Then
            Assert.AreEqual(GameState.Playing, _model.CurrentState);
            Assert.AreEqual(0, _model.Score);
            Assert.AreEqual(1f, Time.timeScale);
        }

        [Test]
        public void Given_ControllerInitialized_When_GameOverCalled_Then_StateIsGameOver_And_TimeScaleIsZero()
        {
            // Given
            _controller.Initialize();

            // When
            _controller.GameOver();

            // Then
            Assert.AreEqual(GameState.GameOver, _model.CurrentState);
            Assert.AreEqual(0f, Time.timeScale);
        }

        [Test]
        public void Given_ControllerInitialized_When_AddScoreCalled_Then_ScoreIsIncreased()
        {
            // Given
            _controller.Initialize();

            // When
            _controller.AddScore(3);

            // Then
            Assert.AreEqual(3, _model.Score);
        }

        [Test]
        public void Given_ControllerInitialized_When_DisposeCalled_Then_DoesNotThrow()
        {
            // When/Then
            Assert.DoesNotThrow(() => _controller.Dispose());
        }
    }
}
