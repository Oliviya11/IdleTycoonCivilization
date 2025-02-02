using Assets.Scripts.Core.Booster;
using Assets.Scripts.Core.Booster.Service;
using Assets.Scripts.Core.ClientsNPCMechanics;
using Assets.Scripts.Core.LevelUpgrade;
using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Core.Orders;
using Assets.Scripts.Core.Sources;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Audio;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Sources;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Infrastracture.States.LoadLevelState;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadLevelState : IPayloadedState<Params>
    {
        IGameStateMachine _gameStateMachine;
        SceneLoader _sceneLoader;
        readonly AllServices _services;
        Hud hud;
        Params @params;
        WinLevelCurtain _curtain;
        BoosterRunner _boosterRunner;

        public class Params
        {
            public string _sceneName;
            public int _level;
            public bool _isLevelUp;

            public Params(string sceneName, int level, bool isLevelUp)
            {
                _sceneName = sceneName;
                _level = level;
                _isLevelUp = isLevelUp;
            }
        }

        public LoadLevelState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services, WinLevelCurtain curtain, BoosterRunner boosterRunner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _curtain = curtain;
            _boosterRunner = boosterRunner;
        }

        void IPayloadedState<Params>.Enter(Params p)
        {
            @params = p;
            if (p._level > 1 && p._isLevelUp)
            {
                _curtain.Show();
            }
            _sceneLoader.Load(p._sceneName, true, OnLoaded);
        }

        void IExitableState.Exit()
        {
           
        }

        private void OnLoaded()
        {
            if (_curtain != null) _curtain.Hide();

            ISourcesManager sourcesManager = CreateSourcesManager();

            LevelStaticData levelStaticData = _services.Single<IStaticDataService>().ForLevel(@params._level);
            IMoneyManager moneyManager = InitMoneyManager(levelStaticData);

            SourcesCollection sources = CreateSources(levelStaticData);

            OrdersCollection ordersCollection = CreateOrders(@params._level);
            GameObject producer = PlaceProducers(levelStaticData);

            ClientsNPCManager clientsNPCManager = CreateSourcesManager(ordersCollection);

            ProducersNPCManager producersNPCManager = CreateProducersManager(levelStaticData, sourcesManager, producer, clientsNPCManager);

            LevelUpgradeManager levelUpgradeManager = new LevelUpgradeManager(levelStaticData.upgradeData, sourcesManager,
                _services.Single<IGameFactory>(), moneyManager, clientsNPCManager, producersNPCManager, sources.click,
                _services.Single<IPersistentProgressService>());

            Booster booster = _services.Single<IPersistentProgressService>().Progress.booster;
            InitHud(levelUpgradeManager, booster);

            InformProgressReaders();

            sources.levelProgress = new SourcesLevelProgress(hud.progressBar, sources, _gameStateMachine);

            _boosterRunner.Construct(_services.Single<IBoosterManager>(), _services.Single<IPersistentProgressService>());

            if (booster != Booster.None)
            {
                _boosterRunner.OnBoosterActivated(booster);
            }
        }

        private IMoneyManager InitMoneyManager(LevelStaticData levelStaticData)
        {
            IPersistentProgressService persistentProgress = _services.Single<IPersistentProgressService>();
            string money = persistentProgress.Progress.money;
            if (string.IsNullOrEmpty(money) || @params._isLevelUp)
            {
                money = levelStaticData.initialMoney;
            }
            IMoneyManager moneyManager = new MoneyManager(money, persistentProgress);
            _services.RegisterSingle<IMoneyManager>(moneyManager);
            return moneyManager;
        }

        private void InitHud(LevelUpgradeManager levelUpgradeManager, Booster booster)
        {
            hud = _services.Single<IGameFactory>().CreateHud().GetComponent<Hud>();
            hud.upgradeButton.onClick.AddListener(levelUpgradeManager.OpenPopup);
            SettingsPopupManager settingsPopupManager = new SettingsPopupManager(_services.Single<IAudioManager>(),
                _services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>());
            hud.settingsButton.onClick.AddListener(settingsPopupManager.OpenPopup);
            hud.booster.Construct(_services.Single<IBoosterManager>());
            if (booster != Booster.None)
            {
                hud.booster.OnBoosterActivated(booster);
            }
            BoosterPopupManager boosterPopupManager = new BoosterPopupManager(_services.Single<IBoosterManager>(), _services.Single<IGameFactory>());
            hud.presentButton.onClick.AddListener(boosterPopupManager.OpenPoup);
        }

        private ProducersNPCManager CreateProducersManager(LevelStaticData levelData, ISourcesManager sourcesManager, GameObject producer, ClientsNPCManager clientsNPCManager)
        {
            GameObject producersManager = _services.Single<IGameFactory>().CreateProducersManager();
            ProducersNPCManager producersNPCManager = producersManager.GetComponent<ProducersNPCManager>();
            producersNPCManager.Construct(clientsNPCManager, new List<ProducerNPC>() { producer.GetComponent<ProducerNPC>() }, 
                sourcesManager, _services.Single<IMoneyManager>(), _services.Single<IGameFactory>(), levelData.producerPosition, levelData.producerRotationAngle,
                _services.Single<IAudioManager>(), _services.Single<IBoosterManager>());
            return producersNPCManager;
        }

        private ClientsNPCManager CreateSourcesManager(OrdersCollection ordersCollection)
        {
            GameObject clientsManager = _services.Single<IGameFactory>().CreateClientsManager();
            ClientsNPCManager clientsNPCManager = clientsManager.GetComponent<ClientsNPCManager>();
            clientsNPCManager.Construct(ordersCollection.places, _services.Single<IGameFactory>(), _services.Single<ISourcesManager>());
            return clientsNPCManager;
        }

        private GameObject PlaceProducers(LevelStaticData levelStaticData)
        {
            return _services.Single<IGameFactory>().CreateProducer(levelStaticData.producerPosition, levelStaticData.producerRotationAngle);
        }

        private SourcesCollection CreateSources(LevelStaticData levelStaticData)
        {
            SourcesCollection sources = CreateSources(@params._level, levelStaticData);
            sources.click.Construct(_services);

            for (int i = 0; i < levelStaticData.sourcesData.Count; ++i) {
                SourceStaticData sourceStaticData = levelStaticData.sourcesData[i];
                Source source = sources.sources[i];
                SourceState sourceState = source.state;
                sourceState.Product = sourceStaticData.product;
                sourceState.Construct(_services);
                if (sourceState.CurrentState == SourceState.State.None)
                {
                    sourceState.EnableAccordingToState(sourceStaticData.initialState);
                }
                source.upgrade = new SourceUpgrade(sourceState, 0, 0,
                    sourceStaticData.productionTime, sourceStaticData.upgrades, 0, 0, sourceStaticData.product,
                    _services.Single<IMoneyManager>());
            }

            return sources;
        }

        private ISourcesManager CreateSourcesManager()
        {
            ISourcesManager sourcesManager = new SourcesManager();
            _services.RegisterSingle<ISourcesManager>(sourcesManager);
            return sourcesManager;
        }

        SourcesCollection CreateSources(int level, LevelStaticData levelStaticData)
        {
            return _services.Single<IGameFactory>().CreateSourcesCollection(level);
        }

        OrdersCollection CreateOrders(int level)
        {
            return _services.Single<IGameFactory>().CreateOrdersCollection(level);
        }

        void InformProgressReaders()
        {
            IGameFactory gameFactory = _services.Single<IGameFactory>();
            IPersistentProgressService persistentProgress = _services.Single<IPersistentProgressService>();

            foreach (ISavedProgressReader progressReader in gameFactory.ProgressReaders)
                progressReader.LoadProgress(persistentProgress.Progress);
        }
    }
}
