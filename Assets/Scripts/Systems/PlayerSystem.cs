using CardGame.Common;
using CardGame.Common.Extensions;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using CardGame.Models;

namespace CardGame.Systems
{
    public class PlayerSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformChangeTurn, NotificationHelper.PerformNotification<ChangeTurnAction>(), Container);
            this.AddObserver(OnPerformDrawCards, NotificationHelper.PerformNotification<DrawCardsAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformChangeTurn, NotificationHelper.PerformNotification<ChangeTurnAction>(), Container);
            this.RemoveObserver(OnPerformDrawCards, NotificationHelper.PerformNotification<DrawCardsAction>(), Container);
        }

        private void OnPerformChangeTurn(object sender, object args)
        {
            var changeTurnAction = args as ChangeTurnAction;
            var match = Container.GetAspect<DataSystem>().Match;
            var player = match.Players[changeTurnAction.TargetPlayerIndex];
            DrawCards(player, 1);
        }

        private void OnPerformDrawCards(object sender, object args)
        {
            var drawCardsAction = args as DrawCardsAction;
            drawCardsAction.Cards = drawCardsAction.Player.Deck.Draw(drawCardsAction.Amount);
            drawCardsAction.Player.Hand.AddRange(drawCardsAction.Cards);
        }

        private void DrawCards(Player player, int amount)
        {
            var drawCardsAction = new DrawCardsAction(player, amount);
            Container.AddReaction(drawCardsAction);
        }
    }
}
