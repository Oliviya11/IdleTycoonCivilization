using Assets.Scripts.Core.ClientsNPCMechanics;
using Assets.Scripts.Core.Orders;
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
            ISourcesManager sourcesManager = new SourcesManager();
            _services.RegisterSingle<ISourcesManager>(sourcesManager);

            LevelStaticData levelStaticData = _services.Single<IStaticDataService>().ForLevel(1);
            SourcesCollection sources = CreateSources(1, levelStaticData);
            sources.click.Construct(_services);
            SourceState source = sources.sources[0].state;
            source.Construct(_services);
            source.EnableAccordingToState(source.InitialState);

            OrdersCollection ordersCollection = CreateOrders(1);

            GameObject producer = _services.Single<IGameFactory>().CreateProducer(levelStaticData.producerPosition, levelStaticData.producerRotationAngle);

            GameObject clientsSpawner = _services.Single<IGameFactory>().CreateClientsManager();
            ClientsNPCManager clientsNPCManager = clientsSpawner.GetComponent<ClientsNPCManager>();
            clientsNPCManager.Construct(ordersCollection.places, _services.Single<IGameFactory>(), _services.Single<ISourcesManager>());

            GameObject producersSpawner = _services.Single<IGameFactory>().CreateProducersManager();
            producersSpawner.GetComponent<ProducersNPCManager>().Construct(clientsNPCManager, new List<ProducerNPC>() { producer.GetComponent<ProducerNPC>() }, sourcesManager);

            _services.Single<IGameFactory>().CreateHud(); 
            _gameStateMachine.Enter<GameLoopState>();
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
