using Assets.Scripts.Infrastracture.AssetManagement;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.Services.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastracture.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        void IState.Enter()
        {
            _sceneLoader.Load(Initial, false, onLoaded: EnterMainMenu);
        }

        void IExitableState.Exit()
        {

        }

        private void RegisterServices()
        {
            AllServices.Container.RegisterSingle<IInputService>(new InputService());

            IAssetProvider assetProvider = new AssetProvider();
            _services.RegisterSingle<IAssetProvider>(assetProvider);

            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>()));
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle<IStaticDataService>(staticData);

            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));
        }

        private void EnterMainMenu()
        {
            _stateMachine.Enter<MainMenuState>();
        }
    }
}
