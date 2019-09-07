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

namespace CardGame.Components
{
    public class DrawCardsView : MonoBehaviour, IOnEventCallback
    {
        [Required] [SerializeField] private GameObject cardViewPrefab = null;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.AddObserver(OnPrepareDrawCards, NotificationHelper.PrepareNotification<DrawCardsAction>());
            }
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.RemoveObserver(OnPrepareDrawCards, NotificationHelper.PrepareNotification<DrawCardsAction>());
            }
        }

        public void OnEvent(EventData photonEvent)
        {

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
            var boardView = GetComponent<BoardView>();
            var playerView = boardView.PlayerViews[drawAction.Player.Index];

            for (int i = 0; i < drawAction.Cards.Count; i++)
            {
                int deckSize = action.Player.Deck.Count + drawAction.Cards.Count - (i + 1);
                playerView.DeckView.ShowDeckSize((float)deckSize / Models.Player.MaxDeck);

                var cardView = Instantiate(cardViewPrefab).GetComponent<CardView>();
                cardView.Card = drawAction.Cards[i];
                cardView.transform.ResetParent(playerView.HandView.transform);
                cardView.transform.position = playerView.DeckView.TopCard.position;
                cardView.transform.rotation = playerView.DeckView.TopCard.rotation;
                cardView.gameObject.SetActive(true);

                bool showPreview = false; // Change dependant on who is drawing
                var addCard = playerView.HandView.AddCard(cardView.transform, showPreview);

                while (addCard.MoveNext()) { yield return null; }
            }
        }
    }
}
