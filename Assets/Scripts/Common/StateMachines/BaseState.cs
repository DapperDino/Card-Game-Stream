namespace CardGame.Common.StateMachines
{
    public abstract class BaseState : Aspect, IState
    {
        public virtual void Enter() { }
        public virtual bool CanTransition(IState other) { return true; }
        public virtual void Exit() { }
    }
}
