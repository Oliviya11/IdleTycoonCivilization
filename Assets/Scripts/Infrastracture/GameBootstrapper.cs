using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastracture
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] MainMenu mainMenuPrefab;
        private Game _game;

        public void Awake()
        {
            _game = new Game(this, mainMenuPrefab);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}
