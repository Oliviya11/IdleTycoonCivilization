using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadProgressState : IState
    {
        const string LEVEL_SCENE_NAME = "Main";
        readonly IGameStateMachine _gameStateMachine;
        readonly AllServices _services;

        public LoadProgressState(IGameStateMachine gameStateMachine, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        void IState.Enter()
        {
            _gameStateMachine.Enter<LoadLevelState, LoadLevelState.Params>(new LoadLevelState.Params(LEVEL_SCENE_NAME, 2));
        }

        void IExitableState.Exit()
        {
          
        }
    }
}
