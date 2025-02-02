using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services;
using UnityEngine;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.Services.Audio;
using Assets.Scripts.Core.Booster;

namespace Assets.Scripts.Infrastracture
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] MainMenu mainMenuPrefab;
        [SerializeField] WinLevelCurtain winLevelCurtainPrefab;
        [SerializeField] AudioManager.Settings soundManagerSettings;
        [SerializeField] BoosterRunner boosterRunner;
        
        private Game _game;

        public void Awake()
        {
            _game = new Game(this, mainMenuPrefab, Instantiate(winLevelCurtainPrefab), soundManagerSettings, boosterRunner);

            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}
