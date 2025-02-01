using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastracture
{
    public class Game
    {
        public IGameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, MainMenu mainMenuPrefab, WinLevelCurtain winLevelCurtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, mainMenuPrefab, winLevelCurtain);
        }
    }
}
