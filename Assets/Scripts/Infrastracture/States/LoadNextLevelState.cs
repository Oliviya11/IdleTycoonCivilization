using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services;
using Assets.Scripts.Services.StaticData;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.States
{
    public class LoadNextLevelState : IState
    {
        readonly AllServices _services;
        readonly IGameStateMachine _gameStateMachine;

        public LoadNextLevelState(IGameStateMachine gameStateMachine, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        public void Enter()
        {
            IPersistentProgressService persistentProgressService = _services.Single<IPersistentProgressService>();
            IStaticDataService staticDataService = _services.Single<IStaticDataService>();

            persistentProgressService.Progress.clients = 1;
            persistentProgressService.Progress.producers = 0;
            persistentProgressService.Progress.appliedLevelUpgrades.Clear();

            int level = persistentProgressService.Progress.level + 1;

            if (staticDataService.GetMaxLevels() == level)
            {
                EndLevelsPopup.OpenPopup(_services.Single<IGameFactory>(), Vector3.zero);
            }
            else
            {
                persistentProgressService.Progress.sources = null;
                _gameStateMachine.Enter<LoadLevelState, LoadLevelState.Params>(new LoadLevelState.Params(LoadProgressState.LEVEL_SCENE_NAME, (++persistentProgressService.Progress.level) + 1, true));
            }
        }

        public void Exit()
        {
            
        }
    }
}
