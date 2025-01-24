using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private async void OnLoaded()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}
