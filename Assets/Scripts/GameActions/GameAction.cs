using CardGame.Common;
using CardGame.Common.Notifications;
using CardGame.Models;

namespace CardGame.GameActions
{
    public class GameAction
    {
        public Player Player { get; set; }
        public int Priority { get; set; }
        public int OrderOfPlay { get; set; }
        public bool IsCanceled { get; protected set; }
        public Phase PreparePhase { get; protected set; }
        public Phase PerformPhase { get; protected set; }
        public Phase CancelPhase { get; protected set; }

        public GameAction()
        {
            PreparePhase = new Phase(this, OnPrepareKeyFrame);
            PerformPhase = new Phase(this, OnPerformKeyFrame);
            CancelPhase = new Phase(this, OnCancelKeyFrame);
        }

        public virtual void Cancel() => IsCanceled = true;

        protected virtual void OnPrepareKeyFrame(IContainer game)
        {
            var notificationName = $"{GetType().Name}.OnPrepare";
            game.PostNotification(notificationName, this);
        }

        protected virtual void OnPerformKeyFrame(IContainer game)
        {
            var notificationName = $"{GetType().Name}.OnPerform";
            game.PostNotification(notificationName, this);
        }

        protected virtual void OnCancelKeyFrame(IContainer game)
        {
            var notificationName = $"{GetType().Name}.OnCancel";
            game.PostNotification(notificationName, this);
        }
    }
}
