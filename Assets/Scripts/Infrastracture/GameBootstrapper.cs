using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services;
using UnityEngine;
using Assets.Scripts.GUI.Popups;

namespace Assets.Scripts.Infrastracture
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] MainMenu mainMenuPrefab;
        [SerializeField] WinLevelCurtain winLevelCurtainPrefab;
        private Game _game;

        public void Awake()
        {
            _game = new Game(this, mainMenuPrefab, Instantiate(winLevelCurtainPrefab));

            AllServices.Container.RegisterSingle<IInputService>(new InputService());

            _game.StateMachine.Enter<BootstrapState>();

            /*
            BigNumber a = new BigNumber("60.5M");
            BigNumber b = new BigNumber("1.2B");

            Debug.LogError($"a = {a}");   // 2.50M
            Debug.LogError($"b = {b}");   // 1.20B
            Debug.LogError($"a + b = {a + b}");  // 1.20B
            Debug.LogError($"b - a = {b - a}");  // 1.20B - 2.5M = 1.1975B
            Debug.LogError($"a * b = {a * b}");  // 2.5M * 1.2B = 3.00Qa
            Debug.LogError($"b / a = {b / a}");  // 
            */
            

            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (AllServices.Container.Single<IInputService>() == null) return;
            AllServices.Container.Single<IInputService>().ProcessInput();
        }
    }
}
