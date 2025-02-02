using Assets.Scripts.Core.Booster;
using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Audio;

namespace Assets.Scripts.Infrastracture
{
    public class Game
    {
        public IGameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, MainMenu mainMenuPrefab, WinLevelCurtain winLevelCurtain, 
            AudioManager.Settings soundManagerSettings, BoosterRunner boosterRunner)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, mainMenuPrefab, 
                winLevelCurtain, soundManagerSettings, boosterRunner);
        }
    }
}
