using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services;
using UnityEngine;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.Services.Audio;

namespace Assets.Scripts.Infrastracture
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] MainMenu mainMenuPrefab;
        [SerializeField] WinLevelCurtain winLevelCurtainPrefab;
        [SerializeField] AudioManager.Settings soundManagerSettings;
        
        private Game _game;

        public void Awake()
        {
            _game = new Game(this, mainMenuPrefab, Instantiate(winLevelCurtainPrefab), soundManagerSettings);

            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}
