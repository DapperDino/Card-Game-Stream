using CardGame.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Components
{
    public class HandView : MonoBehaviour
    {
        private List<Card> cards = new List<Card>();

        public IEnumerator AddCard(Transform card, bool showPreview)
        {
            if (showPreview)
            {
                var preview = ShowPreview(card);
                while (preview.MoveNext()) { yield return null; }
            }

            cards.Add(card);

            var layout = LayOutCards();
            while (layout.MoveNext()) { yield return null; }
        }
    }
}
