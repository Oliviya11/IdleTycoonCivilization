using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services;
using UnityEngine;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.SaveLoad;

namespace Assets.Scripts.Infrastracture
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] MainMenu mainMenuPrefab;
        [SerializeField] WinLevelCurtain winLevelCurtainPrefab;
        const float START_AUTO_SAVE = 1f;
        const float AUTO_SAVE_INTERVAL = 10f;
        private Game _game;

        public void Awake()
        {
            _game = new Game(this, mainMenuPrefab, Instantiate(winLevelCurtainPrefab));

            _game.StateMachine.Enter<BootstrapState>();

            InvokeRepeating(nameof(SaveProgress), START_AUTO_SAVE, AUTO_SAVE_INTERVAL);

            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (AllServices.Container.Single<IInputService>() == null) return;
            AllServices.Container.Single<IInputService>().ProcessInput();
        }

        void OnApplicationQuit()
        {
            SaveProgress();
        }

        void OnApplicationPause()
        {
            SaveProgress();
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) SaveProgress();
        }

        void SaveProgress()
        {
            if (AllServices.Container.Single<ISaveLoadService>() == null) return;
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }
    }
}
