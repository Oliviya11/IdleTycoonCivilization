using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using UnityEngine;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.SaveLoad;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadProgressState : IState
    {
        public const string LEVEL_SCENE_NAME = "Main";
        readonly IGameStateMachine _gameStateMachine;
        readonly AllServices _services;

        public LoadProgressState(IGameStateMachine gameStateMachine, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        void IState.Enter()
        {
            IPersistentProgressService persistentProgressService = _services.Single<IPersistentProgressService>();
            _gameStateMachine.Enter<LoadLevelState, LoadLevelState.Params>(new LoadLevelState.Params(LEVEL_SCENE_NAME, persistentProgressService.Progress.level + 1, false));
        }

        void IExitableState.Exit()
        {
          
        }
    }
}
