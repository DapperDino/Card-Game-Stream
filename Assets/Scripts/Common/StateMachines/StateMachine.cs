namespace CardGame.Common.StateMachines
{
    public class StateMachine : Aspect
    {
        public IState CurrentState { get; private set; }
        public IState PreviousState { get; private set; }

        public IState ChangeState<T>() where T : class, IState, new()
        {
            IState fromState = CurrentState;

            T toState = Container.GetAspect<T>() ?? Container.AddAspect<T>();

            if (fromState != null)
            {
                if (fromState == toState || !fromState.CanTransition(toState)) { return toState; }

                fromState.Exit();
            }

            CurrentState = toState;
            PreviousState = fromState;

            toState.Enter();

            return toState;
        }
    }

    public static class StateMachineExtensions
    {
        public static void ChangeState<T>(this IContainer container) where T : class, IState, new()
        {
            var stateMachine = container.GetAspect<StateMachine>();

            if (stateMachine == null) { return; }

            stateMachine.ChangeState<T>();
        }
    }
}
