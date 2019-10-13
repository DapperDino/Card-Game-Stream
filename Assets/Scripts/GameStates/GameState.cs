using CardGame.Common;
using CardGame.Common.StateMachines;
using CardGame.Systems;
using CardGame.Common.Notifications;

namespace CardGame.GameStates
{
    public class GameState : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnBeginSequence, ActionSystem.OnBeginSequenceNotification);
            this.AddObserver(OnCompleteAllActions, ActionSystem.OnCompleteNotification);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnBeginSequence, ActionSystem.OnBeginSequenceNotification);
            this.RemoveObserver(OnCompleteAllActions, ActionSystem.OnCompleteNotification);
        }

        private void OnBeginSequence(object sender, object args) => Container.ChangeState<SequenceState>();
        private void OnCompleteAllActions(object sender, object args)
        {
            if (Container.GetAspect<VictorySystem>().IsGameOver())
            {
                Container.ChangeState<GameOverState>();
            }
            else
            {
                Container.ChangeState<PlayerIdleState>();
            }
        }
    }
}
