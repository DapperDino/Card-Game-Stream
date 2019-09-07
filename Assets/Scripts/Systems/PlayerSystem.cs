using CardGame.Common;
using CardGame.Common.Extensions;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using CardGame.Models;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace CardGame.Systems
{
    public class PlayerSystem : Aspect, IObserve
    {
        public void Awake()
        {
            this.AddObserver(OnPerformChangeTurn, NotificationHelper.PerformNotification<ChangeTurnAction>(), Container);
            this.AddObserver(OnPerformDrawCards, NotificationHelper.PerformNotification<DrawCardsAction>(), Container);
            this.AddObserver(OnPerformOverdraw, NotificationHelper.PerformNotification<OverdrawAction>(), Container);
            this.AddObserver(OnPerformFatigue, NotificationHelper.PerformNotification<FatigueAction>(), Container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPerformChangeTurn, NotificationHelper.PerformNotification<ChangeTurnAction>(), Container);
            this.RemoveObserver(OnPerformDrawCards, NotificationHelper.PerformNotification<DrawCardsAction>(), Container);
            this.RemoveObserver(OnPerformOverdraw, NotificationHelper.PerformNotification<OverdrawAction>(), Container);
            this.RemoveObserver(OnPerformFatigue, NotificationHelper.PerformNotification<FatigueAction>(), Container);
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
            var drawAction = args as DrawCardsAction;

            int deckCount = drawAction.Player.Deck.Count;
            int fatigueCount = Mathf.Max(drawAction.Amount - deckCount, 0);
            for (int i = 0; i < fatigueCount; i++)
            {
                var fatigueAction = new FatigueAction(drawAction.Player);
                Container.AddReaction(fatigueAction);
            }

            int roomInHand = Player.MaxHand - drawAction.Player.Hand.Count;
            int overdraw = Mathf.Max((drawAction.Amount - fatigueCount) - roomInHand, 0);

            if (overdraw > 0)
            {
                var overdrawAction = new OverdrawAction(drawAction.Player, overdraw);
                Container.AddReaction(overdrawAction);
            }

            int drawCount = drawAction.Amount - fatigueCount - overdraw;
            drawAction.Cards = drawAction.Player.Deck.Draw(drawCount);
            drawAction.Player.Hand.AddRange(drawAction.Cards);

            int[] cardIndices = new int[drawAction.Cards.Count];
            for (int i = 0; i < drawAction.Cards.Count; i++)
            {
                cardIndices[i] = drawAction.Cards[i].Id;
            }

            byte targetPlayerIndex = drawAction.Player.Index;
            var raiseEventoptions = new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others };
            var sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(EventCodes.OnCardsDraw, new object[] { targetPlayerIndex, drawAction.Player.Deck.Count, cardIndices }, raiseEventoptions, sendOptions);
        }

        private void OnPerformOverdraw(object sender, object args)
        {
            var overdrawAction = args as OverdrawAction;
            overdrawAction.Cards = overdrawAction.Player.Deck.Draw(overdrawAction.Amount);
            overdrawAction.Player.Graveyard.AddRange(overdrawAction.Cards);

            int[] cardIndices = new int[overdrawAction.Cards.Count];
            for (int i = 0; i < overdrawAction.Cards.Count; i++)
            {
                cardIndices[i] = overdrawAction.Cards[i].Id;
            }

            byte targetPlayerIndex = overdrawAction.Player.Index;
            var raiseEventoptions = new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others };
            var sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(EventCodes.OnOverdraw, new object[] { targetPlayerIndex, overdrawAction.Player.Deck.Count, cardIndices }, raiseEventoptions, sendOptions);
        }

        private void OnPerformFatigue(object sender, object args)
        {
            var fatigueAction = args as FatigueAction;
            fatigueAction.Player.Fatigue++;

            var raiseEventoptions = new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others };
            var sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(EventCodes.OnFatigue, fatigueAction.Player.Fatigue, raiseEventoptions, sendOptions);
        }

        private void DrawCards(Player player, int amount)
        {
            var drawCardsAction = new DrawCardsAction(player, amount);
            Container.AddReaction(drawCardsAction);
        }
    }
}
