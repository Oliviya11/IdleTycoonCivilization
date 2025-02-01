namespace Assets.Scripts.Infrastracture.States
{
    public class LoadNextLevelState : IState
    {
        IGameStateMachine gameStateMachine;
        public LoadNextLevelState(IGameStateMachine gameStateMachine)
        {

        }
        public void Enter()
        {
            //increase level
            //call LoadProgressState
        }

        public void Exit()
        {
            
        }
    }
}
