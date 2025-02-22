﻿using Assets.Scripts.Core.Booster;
using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastracture.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services, MainMenu mainMenuPrefab, WinLevelCurtain winLevelCurtain, 
            Services.Audio.AudioManager.Settings soundManagerSettings, BoosterRunner boosterRunner)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, soundManagerSettings),
                [typeof(MainMenuState)] = new MainMenuState(this, sceneLoader, mainMenuPrefab, services),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, services, winLevelCurtain, boosterRunner),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(LoadNextLevelState)] = new LoadNextLevelState(this, services),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
          _states[typeof(TState)] as TState;
    }
}
