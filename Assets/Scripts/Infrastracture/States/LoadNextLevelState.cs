using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadNextLevelState : IState
    {
        AllServices _services;
        IGameStateMachine _gameStateMachine;

        public LoadNextLevelState(IGameStateMachine gameStateMachine, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        public void Enter()
        {
            //increase level
            IPersistentProgressService persistentProgressService = _services.Single<IPersistentProgressService>();
            persistentProgressService.Progress.sources = null;
            _gameStateMachine.Enter<LoadLevelState, LoadLevelState.Params>(new LoadLevelState.Params(LoadProgressState.LEVEL_SCENE_NAME, (++persistentProgressService.Progress.level) + 1));
        }

        public void Exit()
        {
            
        }
    }
}
