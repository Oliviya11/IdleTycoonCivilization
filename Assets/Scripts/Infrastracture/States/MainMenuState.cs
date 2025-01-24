using Assets.Scripts.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.States
{
    public class MainMenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly MainMenu _mainMenuPrefab;
        private MainMenu _mainMenu;

        public MainMenuState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, MainMenu mainMenuPrefab)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuPrefab = mainMenuPrefab;
        }

        public void Enter()
        {
            _sceneLoader.Load(MainMenu, onLoaded: OnLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            _mainMenu = GameObject.Instantiate(_mainMenuPrefab);
            _mainMenu.PlayButton.onClick.AddListener(OnPlayClick);
        }

        private void OnPlayClick()
        {
            _gameStateMachine.Enter<LoadProgressState>();
            _mainMenu.PlayButton.onClick.RemoveListener(OnPlayClick);
        }
    }
}
