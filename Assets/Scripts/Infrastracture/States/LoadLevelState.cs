using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Sources;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        IGameStateMachine _gameStateMachine;
        SceneLoader _sceneLoader;

        public LoadLevelState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
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
            LevelStaticData levelStaticData = AllServices.Container.Single<IStaticDataService>().ForLevel(1);
            SourcesCollection sources = CreateSources(1, levelStaticData);
            SourceState source = sources.sources[0].state;
            source.EnableAccordingToState(source.InitialState);
            GameObject producer = AllServices.Container.Single<IGameFactory>().CreateProducer(levelStaticData.producerPosition, levelStaticData.producerRotationAngle);
            AllServices.Container.Single<IGameFactory>().CreateHud();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private SourcesCollection CreateSources(int level, LevelStaticData levelStaticData)
        {
            return AllServices.Container.Single<IGameFactory>().CreateSourcesCollection(level);
        }
    }
}
