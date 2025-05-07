using Controllers;
using Interfaces;
using Models;
using Moq;
using NUnit.Framework;
using Signals;
using UnityEngine;
using Zenject;

namespace Tests.EditMode
{
    public class BirdSpawnerTests : ZenjectUnitTestFixture
    {
        private Mock<IBirdController> mockBirdController;
        private Mock<BirdController.Factory> mockBirdFactory;

        private BirdSpawner birdSpawner;

        private Transform background;

        [SetUp]
        public void Given_BirdSpawnerSetup_When_TestBegins_Then_ConfigureContainer()
        {
            mockBirdController = new Mock<IBirdController>();

            background = new GameObject("Background").transform;

            Container.Bind<SignalBus>().AsSingle();
            Container.Bind<Transform>().WithId("Background").FromInstance(background).AsSingle();

            var fakeFactory = new FakeBirdFactory(mockBirdController.Object);
            Container.BindFactory<BirdController, FakeBirdFactory>().FromInstance(fakeFactory);

            birdSpawner = Container.Instantiate<BirdSpawner>();

            birdSpawner.Initialize();
        }


        [TearDown]
        public void Cleanup()
        {
            UnityEngine.Object.DestroyImmediate(background.gameObject);
        }

        [Test]
        public void Given_PlayingState_When_GameStateChanges_Then_BirdIsSpawnedAndParented()
        {
            // When the game state changes to Playing
            Container.Resolve<SignalBus>().Fire(new GameStateChangedSignal { NewState = GameState.Playing });

            // Then
            mockBirdFactory.Verify(f => f.Create(), Times.Once); // Verifying the bird controller creation
            mockBirdController.Verify(b => b.SetParent(background), Times.Once); // Verifying the SetParent call
        }

        [Test]
        public void Given_NonPlayingState_When_GameStateChanges_Then_BirdIsNotSpawned()
        {
            // When the game state changes to non-playing (Waiting or GameOver)
            Container.Resolve<SignalBus>().Fire(new GameStateChangedSignal { NewState = GameState.Waiting });
            Container.Resolve<SignalBus>().Fire(new GameStateChangedSignal { NewState = GameState.GameOver });

            // Then
            mockBirdFactory.Verify(f => f.Create(), Times.Never); // The bird controller should not be created
            mockBirdController.Verify(b => b.SetParent(It.IsAny<Transform>()),
                Times.Never); // No SetParent call should be made
        }

        public class FakeBirdFactory : BirdController.Factory
        {
            private readonly IBirdController _controllerToReturn;

            public FakeBirdFactory(IBirdController controllerToReturn)
            {
                _controllerToReturn = controllerToReturn;
            }

            public override BirdController Create()
            {
                return (BirdController)_controllerToReturn;
            }
        }
    }
}