using CardGame.Common;
using CardGame.Common.StateMachines;
using NUnit.Framework;

namespace Tests
{
    public class StateMachineTests
    {
        private class TestState : BaseState
        {
            public bool CalledEnter { get; set; }
            public bool CalledExit { get; set; }
            public bool IsLocked { get; set; }

            public override void Enter() => CalledEnter = true;
            public override void Exit() => CalledExit = true;
            public override bool CanTransition(IState other) => !IsLocked;
        }

        private class AltTestState : BaseState { }

        private IContainer container = null;
        private StateMachine stateMachine = null;

        [SetUp]
        public void TestSetup()
        {
            container = new Container();
            stateMachine = container.AddAspect<StateMachine>();
        }

        [Test]
        public void ChangingToExistingState_StateIsChanged()
        {
            var testState = container.AddAspect<TestState>();

            stateMachine.ChangeState<TestState>();

            Assert.AreSame(stateMachine.CurrentState, testState);
        }

        [Test]
        public void ChangingToNonExistingState_StateIsAdded()
        {
            stateMachine.ChangeState<TestState>();

            Assert.IsTrue(stateMachine.CurrentState is TestState);
        }

        [Test]
        public void ChangingState_EnterIsCalledOnNewState()
        {
            stateMachine.ChangeState<TestState>();
            var state = stateMachine.CurrentState as TestState;

            Assert.IsTrue(state.CalledEnter);
        }

        [Test]
        public void ChangingState_ExitIsCalledOnOldState()
        {
            stateMachine.ChangeState<TestState>();
            var state = stateMachine.CurrentState as TestState;
            stateMachine.ChangeState<AltTestState>();

            Assert.IsTrue(state.CalledExit);
        }

        [Test]
        public void GettingPreviousState_ReturnsPreviousState()
        {
            var previousState = stateMachine.ChangeState<TestState>();

            stateMachine.ChangeState<AltTestState>();

            Assert.AreSame(stateMachine.PreviousState, previousState);
        }

        [Test]
        public void ChangingStateWhenCanTransitionReturnsFalse_StateIsNotChanged()
        {
            var stateBeforeLock = container.AddAspect<TestState>();

            stateMachine.ChangeState<TestState>();
            stateBeforeLock.IsLocked = true;
            stateMachine.ChangeState<AltTestState>();

            Assert.AreSame(stateMachine.CurrentState, stateBeforeLock);
        }

        [Test]
        public void ChangingToTheSameState_TransitionIsIgnored()
        {
            var currentState = container.AddAspect<TestState>();

            stateMachine.ChangeState<TestState>();
            currentState.CalledEnter = false;
            stateMachine.ChangeState<TestState>();

            Assert.IsFalse(currentState.CalledEnter);
        }
    }
}

