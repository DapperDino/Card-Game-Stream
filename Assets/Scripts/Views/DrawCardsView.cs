using UnityEngine;
using System.Collections;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using Photon.Pun;
using CardGame.Common;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Sirenix.OdinInspector;
using CardGame.Common.Extensions;
using CardGame.Cards;
using System.Collections.Generic;

namespace CardGame.Views
{
    public class DrawCardsView : MonoBehaviour, IOnEventCallback
    {
        [Required] [SerializeField] private GameObject cardViewPrefab = null;
        [Required] [SerializeField] private CardDatabase cardDatabase = null;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.AddObserver(OnPrepareDrawCards, NotificationHelper.PrepareNotification<DrawCardsAction>());
                this.AddObserver(OnPrepareDrawCards, NotificationHelper.PrepareNotification<OverdrawAction>());
            }
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.RemoveObserver(OnPrepareDrawCards, NotificationHelper.PrepareNotification<DrawCardsAction>());
                this.RemoveObserver(OnPrepareDrawCards, NotificationHelper.PrepareNotification<DrawCardsAction>());
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case EventCodes.OnCardsDraw:
                    ReceiveDrawCardData(photonEvent.CustomData);
                    return;

                case EventCodes.OnOverdraw:
                    ReceiveDrawCardData(photonEvent.CustomData, true);
                    return;
            }
        }

        private void ReceiveDrawCardData(object customData, bool isOverdraw = false)
        {
            object[] eventData = (object[])customData;
            var targetPlayerIndex = (byte)eventData[0];
            var deckSize = (int)eventData[1];
            var cardsIndices = (int[])eventData[2];

            var cards = new List<Card>();
            for (int i = 0; i < cardsIndices.Length; i++)
            {
                cards.Add(cardDatabase.GetCardById(cardsIndices[i]).CreateInstance(targetPlayerIndex));
            }

            StartCoroutine(DisplayDrawCards(targetPlayerIndex, deckSize, isOverdraw, cards));
        }

        private void OnPrepareDrawCards(object sender, object args)
        {
            var drawAction = args as DrawCardsAction;
            drawAction.PerformPhase.Viewer = DrawCardsViewer;
        }

        private IEnumerator DrawCardsViewer(IContainer game, GameAction action)
        {
            yield return true;

            var drawAction = action as DrawCardsAction;

            var isOverdrawAction = action is OverdrawAction;

            yield return StartCoroutine(DisplayDrawCards(drawAction.Player.Index, drawAction.Player.Deck.Count, isOverdrawAction, drawAction.Cards));
        }

        private IEnumerator DisplayDrawCards(byte targetPlayerIndex, int deckSize, bool isOverdraw, List<Card> cards)
        {
            var boardView = GetComponent<BoardView>();

            var myTurnIndex = (byte)PhotonNetwork.LocalPlayer.CustomProperties["TurnIndex"];
            var isAllyDrawing = myTurnIndex == targetPlayerIndex;
            var playerView = isAllyDrawing ? boardView.PlayerViews[0] : boardView.PlayerViews[1];

            for (int i = 0; i < cards.Count; i++)
            {
                playerView.DeckView.SetDeckSize((float)deckSize / Models.Player.MaxDeck);

                var cardView = Instantiate(cardViewPrefab).GetComponent<CardView>();
                cardView.Card = cards[i];
                cardView.transform.ResetParent(playerView.HandView.transform);
                cardView.transform.position = playerView.DeckView.TopCard.position;
                cardView.transform.rotation = playerView.DeckView.TopCard.rotation;
                cardView.gameObject.SetActive(true);

                var addCard = playerView.HandView.AddCard(cardView.transform, isAllyDrawing, isOverdraw);

                while (addCard.MoveNext()) { yield return null; }
            }
        }
    }
}
