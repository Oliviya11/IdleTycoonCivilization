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
using Assets.Scripts.Services.Inputs;
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
        WinLevelCurtain curtain;

        public class Params
        {
            public string sceneName;
            public int level;

            public Params(string sceneName, int level)
            {
                this.sceneName = sceneName;
                this.level = level;
            }
        }

        public LoadLevelState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services, WinLevelCurtain curtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            this.curtain = curtain;
        }

        void IPayloadedState<Params>.Enter(Params p)
        {
            @params = p;
            curtain.Show();
            _sceneLoader.Load(p.sceneName, OnLoaded);
        }

        void IExitableState.Exit()
        {
           hud.upgradeButton.onClick.RemoveAllListeners();
        }

        private void OnLoaded()
        {
            if (curtain != null) curtain.Hide();

            ISourcesManager sourcesManager = CreateSourcesManager();

            LevelStaticData levelStaticData = _services.Single<IStaticDataService>().ForLevel(@params.level);

            IMoneyManager moneyManager = new MoneyManager(levelStaticData.initialMoney);
            _services.RegisterSingle<IMoneyManager>(moneyManager);

            SourcesCollection sources = CreateSources(levelStaticData);

            OrdersCollection ordersCollection = CreateOrders(@params.level);
            GameObject producer = PlaceProducers(levelStaticData);

            ClientsNPCManager clientsNPCManager = CreateSourcesManager(ordersCollection);
            ProducersNPCManager producersNPCManager = CreateProducersManager(levelStaticData, sourcesManager, producer, clientsNPCManager);

            LevelUpgradeManager levelUpgradeManager = new LevelUpgradeManager(levelStaticData.upgradeData, sourcesManager, 
                _services.Single<IGameFactory>(), moneyManager, clientsNPCManager, producersNPCManager, sources.click);

            InitHud(levelUpgradeManager);

            sources.levelProgress = new SourcesLevelProgress(hud.progressBar, sources, _gameStateMachine);
        }

        private void InitHud(LevelUpgradeManager levelUpgradeManager)
        {
            hud = _services.Single<IGameFactory>().CreateHud().GetComponent<Hud>();
            hud.upgradeButton.onClick.AddListener(levelUpgradeManager.OpenPopup);
        }

        private ProducersNPCManager CreateProducersManager(LevelStaticData levelData, ISourcesManager sourcesManager, GameObject producer, ClientsNPCManager clientsNPCManager)
        {
            GameObject producersManager = _services.Single<IGameFactory>().CreateProducersManager();
            ProducersNPCManager producersNPCManager = producersManager.GetComponent<ProducersNPCManager>();
            producersNPCManager.Construct(clientsNPCManager, new List<ProducerNPC>() { producer.GetComponent<ProducerNPC>() }, 
                sourcesManager, _services.Single<IMoneyManager>(), _services.Single<IGameFactory>(), levelData.producerPosition, levelData.producerRotationAngle);
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
            SourcesCollection sources = CreateSources(@params.level, levelStaticData);
            sources.click.Construct(_services);

            for (int i = 0; i < levelStaticData.sourcesData.Count; ++i) {
                SourceStaticData sourceStaticData = levelStaticData.sourcesData[i];
                Source source = sources.sources[i];
                SourceState sourceState = source.state;
                sourceState.Product = sourceStaticData.product;
                sourceState.Construct(_services);
                sourceState.EnableAccordingToState(sourceStaticData.initialState);
                source.upgrade = new SourceUpgrade(sourceState, 0, 0,
                    sourceStaticData.productionTime, sourceStaticData.upgrades, 0, 0, sourceStaticData.product,
                    _services.Single<IMoneyManager>());
                source.upgrade.UpgradeTill(0, 0);
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
    }
}
