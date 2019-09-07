using CardGame.Common;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace CardGame.Systems
{
    public class MatchSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformChangeTurn, NotificationHelper.PerformNotification<ChangeTurnAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformChangeTurn, NotificationHelper.PerformNotification<ChangeTurnAction>(), Container);
        }

        public void ChangeTurn()
        {
            var match = Container.GetMatch();
            byte nextPlayerIndex = (byte)(1 - match.CurrentPlayerIndex);
            ChangeTurn(nextPlayerIndex);
        }

        public void ChangeTurn(byte nextPlayerIndex)
        {
            var action = new ChangeTurnAction(nextPlayerIndex);
            Container.Perform(action);
        }

        private void OnPerformChangeTurn(object sender, object args)
        {
            var action = args as ChangeTurnAction;
            var match = Container.GetMatch();
            match.CurrentPlayerIndex = action.TargetPlayerIndex;

            var raiseEventoptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            var sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(EventCodes.OnTurnChanged, match.CurrentPlayerIndex, raiseEventoptions, sendOptions);
        }
    }
}
