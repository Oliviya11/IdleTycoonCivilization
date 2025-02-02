using Assets.Scripts.GUI;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.Services.Audio;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Core.Booster.Service;

namespace Assets.Scripts.Infrastracture.States
{
    public class MainMenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly MainMenu _mainMenuPrefab;
        private MainMenu _mainMenu;
        private AllServices _services;

        public MainMenuState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, MainMenu mainMenuPrefab, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuPrefab = mainMenuPrefab;
            _services = services;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            SetAudio();

            _sceneLoader.Load(MainMenu, false, onLoaded: OnLoaded);
            _services.Single<IBoosterManager>().Load();
        }

        private void SetAudio()
        {
            IAudioManager audioManager = _services.Single<IAudioManager>();
            IPersistentProgressService persistentProgress = _services.Single<IPersistentProgressService>();

            if (persistentProgress.Progress.sound)
            {
                audioManager.UnmuteEffects();
            }
            else
            {
                audioManager.MuteEffects();
            }

            if (persistentProgress.Progress.music)
            {
                audioManager.UnmuteBackgroundMusic();
            }
            else
            {
                audioManager.MuteBackgroundMusic();
            }
        }

        public void Exit()
        {
            _mainMenu.playButton.onClick.RemoveAllListeners();
            _mainMenu.settingsButton.onClick.RemoveAllListeners();
        }

        //TODO redo main menu instantiation
        private void OnLoaded()
        {
            _mainMenu = GameObject.Instantiate(_mainMenuPrefab);
            _mainMenu.playButton.onClick.AddListener(OnPlayClick);
            SettingsPopupManager settingsPopupManager = new SettingsPopupManager(_services.Single<IAudioManager>(),
                _services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>());
            _mainMenu.settingsButton.onClick.AddListener(settingsPopupManager.OpenPopup);
        }

        private void OnPlayClick()
        {
            _gameStateMachine.Enter<LoadProgressState>();
            _mainMenu.playButton.onClick.RemoveListener(OnPlayClick);
        }

        private void LoadProgressOrInitNew()
        {
            _services.Single<IPersistentProgressService>().Progress =
              _services.Single<ISaveLoadService>().LoadProgress();
        }
    }
}
