namespace CardGame.Common.StateMachines
{
    public interface IState : IAspect
    {
        void Enter();
        bool CanTransition(IState other);
        void Exit();
    }
}
