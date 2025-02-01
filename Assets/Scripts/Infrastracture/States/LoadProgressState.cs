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

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadProgressState : IState
    {
        const string LEVEL_SCENE_NAME = "Main";
        readonly IGameStateMachine gameStateMachine;
        readonly AllServices services;

        public LoadProgressState(IGameStateMachine gameStateMachine, AllServices services)
        {
            this.gameStateMachine = gameStateMachine;
            this.services = services;
        }

        void IState.Enter()
        {
            gameStateMachine.Enter<LoadLevelState, LoadLevelState.Params>(new LoadLevelState.Params(LEVEL_SCENE_NAME, 1));
        }

        void IExitableState.Exit()
        {
          
        }
    }
}
