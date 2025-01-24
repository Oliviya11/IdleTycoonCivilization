using Assets.Scripts.Services;
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
            _sceneLoader.Load(Initial, onLoaded: EnterMainMenu);
        }

        void IExitableState.Exit()
        {

        }

        private void RegisterServices()
        {

        }

        private void EnterMainMenu()
        {
            _stateMachine.Enter<MainMenuState>();
        }
    }
}
