using CardGame.Common.Notifications;
using CardGame.Common.StateMachines;

namespace CardGame.GameStates
{
    public class PlayerIdleState : BaseState
    {
        private const string OnEnterNotification = "PlayerIdleState.OnEnter";
        private const string OnExitNotification = "PlayerIdleState.OnExit";

        public override void Enter() => this.PostNotification(OnEnterNotification);
        public override void Exit() => this.PostNotification(OnExitNotification);
    }
}
