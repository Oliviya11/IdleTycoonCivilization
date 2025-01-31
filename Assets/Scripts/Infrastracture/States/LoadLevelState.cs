using Assets.Scripts.Core.ClientsNPCMechanics;
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

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        IGameStateMachine _gameStateMachine;
        SceneLoader _sceneLoader;
        readonly AllServices _services;
        Hud hud;

        public LoadLevelState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
        }

        void IPayloadedState<string>.Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        void IExitableState.Exit()
        {
            //hud.upgradeButton.onClick.RemoveAllListeners();
        }

        private void OnLoaded()
        {
            ISourcesManager sourcesManager = CreateSourcesManager();

            LevelStaticData levelStaticData = _services.Single<IStaticDataService>().ForLevel(1);

            IMoneyManager moneyManager = new MoneyManager(levelStaticData.initialMoney);
            _services.RegisterSingle<IMoneyManager>(moneyManager);

            CreateSources(levelStaticData);

            OrdersCollection ordersCollection = CreateOrders(1);
            GameObject producer = PlaceProducers(levelStaticData);

            ClientsNPCManager clientsNPCManager = CreateSourcesManager(ordersCollection);
            CreateProducersManager(sourcesManager, producer, clientsNPCManager);
            InitHud();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitHud()
        {
            hud = _services.Single<IGameFactory>().CreateHud().GetComponent<Hud>();
            hud.upgradeButton.onClick.AddListener(delegate ()
            {
                LevelUpgradePopup.OpenLevelPopUp(new LevelUpgradePopup.Params(), _services.Single<IGameFactory>(), Vector3.zero, delegate (LevelUpgradePopup p) { });
            });
        }

        private void CreateProducersManager(ISourcesManager sourcesManager, GameObject producer, ClientsNPCManager clientsNPCManager)
        {
            GameObject producersManager = _services.Single<IGameFactory>().CreateProducersManager();
            producersManager.GetComponent<ProducersNPCManager>().Construct(clientsNPCManager, new List<ProducerNPC>() { producer.GetComponent<ProducerNPC>() }, 
                sourcesManager, _services.Single<IMoneyManager>());
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

        private void CreateSources(LevelStaticData levelStaticData)
        {
            SourcesCollection sources = CreateSources(1, levelStaticData);
            sources.click.Construct(_services);

            for (int i = 0; i < levelStaticData.sourcesData.Count; ++i) {
                SourceStaticData sourceStaticData = levelStaticData.sourcesData[i];
                Source source = sources.sources[i];
                SourceState sourceState = source.state;
                sourceState.Construct(_services);
                sourceState.EnableAccordingToState(sourceStaticData.initialState);
                sourceState.Product = sourceStaticData.product;
                source.upgrade = new SourceUpgrade(sourceState, 0, 0,
                    sourceStaticData.productionTime, sourceStaticData.upgrades, 0, 0, sourceStaticData.product,
                    _services.Single<IMoneyManager>());
                source.upgrade.UpgradeTill(0, 0);
            }
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
