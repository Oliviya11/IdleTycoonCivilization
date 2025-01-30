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
        readonly IGameStateMachine _gameStateMachine;
        readonly AllServices _services;

        public LoadProgressState(IGameStateMachine gameStateMachine, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        void IState.Enter()
        {
            IMoneyManager moneyManager = new MoneyManager("5");
            _services.RegisterSingle<IMoneyManager>(moneyManager);
            _gameStateMachine.Enter<LoadLevelState, string>("Main");
        }

        void IExitableState.Exit()
        {
          
        }
    }
}
