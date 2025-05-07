using Controllers;
using Models;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Scene Assets")] [SerializeField]
        private Canvas canvas;

        [SerializeField] private Transform background;
        [SerializeField] private Transform pipesParent;
        [SerializeField] private Transform ground;

        [Space] [Header("Models")] [SerializeField]
        private GameModel gameModel;

        [SerializeField] private BirdModel birdModel;
        [SerializeField] private PipesModel pipesModel;

        [Space] [Header("Controllers")] [SerializeField]
        private BirdController birdController;

        [Space] [Header("Views")] [SerializeField]
        private MenuView menuView;

        [SerializeField] private PipesView pipesView;
        [SerializeField] private HUDView hudView;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.BindInstance(canvas).WithId("MainCanvas");
            Container.BindInstance(pipesParent).WithId("PipesParent");
            Container.BindInstance(background).WithId("Background");
            Container.BindInstance(ground).WithId("Ground");

            Container.Bind<GameModel>().FromInstance(gameModel).AsSingle();
            Container.Bind<BirdModel>().FromInstance(birdModel).AsSingle();
            Container.Bind<PipesModel>().FromInstance(pipesModel).AsSingle();


            Container.BindFactory<MenuView, MenuView.StartPanelFactory>().FromComponentInNewPrefab(menuView).AsSingle();
            Container.BindFactory<MenuView.DataModel, MenuView, MenuView.GameOverPanelFactory>()
                .FromComponentInNewPrefab(menuView).AsSingle();
            Container.BindFactory<PipesView, PipesView.Factory>().FromComponentInNewPrefab(pipesView).AsSingle();
            Container.BindFactory<BirdController, BirdController.Factory>().FromComponentInNewPrefab(birdController)
                .AsSingle();
            Container.BindFactory<HUDView.DataModel, HUDView, HUDView.Factory>().FromComponentInNewPrefab(hudView)
                .AsSingle();

            Container.DeclareSignal<ScoreUpdatedSignal>();
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<PlayerFlapSignal>();
            Container.DeclareSignal<AddScoreSignal>();
            Container.DeclareSignal<StartGameRequestSignal>();
            Container.DeclareSignal<GameOverSignal>();

            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PipesController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<HUDController>().AsSingle();
        }
    }
}