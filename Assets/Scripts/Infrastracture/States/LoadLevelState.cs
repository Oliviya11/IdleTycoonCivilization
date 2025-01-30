using Assets.Scripts.Core.ClientsNPCMechanics;
using Assets.Scripts.Core.Orders;
using Assets.Scripts.Core.Sources;
using Assets.Scripts.Core.Sources.Services;
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
            
        }

        private void OnLoaded()
        {
            ISourcesManager sourcesManager = CreateSourcesManager();
            LevelStaticData levelStaticData = CreateSources();

            OrdersCollection ordersCollection = CreateOrders(1);
            GameObject producer = PlaceProducers(levelStaticData);

            ClientsNPCManager clientsNPCManager = CreateSourcesManager(ordersCollection);
            CreateProducersManager(sourcesManager, producer, clientsNPCManager);

            _services.Single<IGameFactory>().CreateHud();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void CreateProducersManager(ISourcesManager sourcesManager, GameObject producer, ClientsNPCManager clientsNPCManager)
        {
            GameObject producersManager = _services.Single<IGameFactory>().CreateProducersManager();
            producersManager.GetComponent<ProducersNPCManager>().Construct(clientsNPCManager, new List<ProducerNPC>() { producer.GetComponent<ProducerNPC>() }, sourcesManager);
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

        private LevelStaticData CreateSources()
        {
            LevelStaticData levelStaticData = _services.Single<IStaticDataService>().ForLevel(1);
            SourcesCollection sources = CreateSources(1, levelStaticData);
            sources.click.Construct(_services);

            for (int i = 0; i < levelStaticData.sourcesData.Count; ++i) {
                SourceStaticData sourceStaticData = levelStaticData.sourcesData[i];
                Source source = sources.sources[i];
                SourceState sourceState = source.state;
                sourceState.Construct(_services);
                sourceState.EnableAccordingToState(sourceStaticData.initialState);
                sourceState.Product = sourceStaticData.product;
                source.upgrade = new SourceUpgrade(sourceStaticData.initialPrice, sourceStaticData.initialProfit,
                    sourceStaticData.productionTime, sourceStaticData.upgrades, 0, 0);
                source.upgrade.UpgradeTill(0, 0);
            }
            return levelStaticData;
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
