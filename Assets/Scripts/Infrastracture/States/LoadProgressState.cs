using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;

        public LoadProgressState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        void IState.Enter()
        {
            _gameStateMachine.Enter<LoadLevelState, string>("Main");
        }

        void IExitableState.Exit()
        {
          
        }
    }
}
