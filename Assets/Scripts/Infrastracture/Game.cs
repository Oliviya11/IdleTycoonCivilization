using Assets.Scripts.GUI;
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

        public Game(ICoroutineRunner coroutineRunner, MainMenu mainMenuPrefab)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, mainMenuPrefab);
        }
    }
}
