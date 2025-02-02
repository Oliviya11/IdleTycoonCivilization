using Assets.Scripts.Core.Booster.Service;
using Assets.Scripts.Infrastracture.AssetManagement;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Audio;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.Services.StaticData;

namespace Assets.Scripts.Infrastracture.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        AudioManager.Settings _soundManagerSettings;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services, AudioManager.Settings soundManagerSettings)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _soundManagerSettings = soundManagerSettings;

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
            AllServices.Container.RegisterSingle<IAudioManager>(new AudioManager(_soundManagerSettings));
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

            InitBoosterManager(staticData);
        }

        private void InitBoosterManager(IStaticDataService staticData)
        {
            IBoosterManager boosterManager = new BoosterManager(staticData.GetBoosters(), 
                _services.Single<IPersistentProgressService>());
            _services.RegisterSingle<IBoosterManager>(boosterManager);
        }

        private void EnterMainMenu()
        {
            _stateMachine.Enter<MainMenuState>();
        }
    }
}
