using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services;
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

            AllServices.Container.RegisterSingle<IInputService>(new InputService());

            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (AllServices.Container.Single<IInputService>() == null) return;
            AllServices.Container.Single<IInputService>().ProcessInput();
        }
    }
}
