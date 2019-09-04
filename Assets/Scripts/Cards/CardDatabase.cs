using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Card Database", menuName = "Cards/Card Database")]
    public class CardDatabase : ScriptableObject
    {
        [SerializeField] private CardTemplate[] cards = new CardTemplate[0];

        [Button]
        public void GetAllCards()
        {
            cards = Resources.LoadAll<CardTemplate>("Cards");
            cards = cards.OrderBy(card => card.Id).ToArray();

            for (int i = 0; i < cards.Length; i++)
            {
                if(cards[i].Id == -1)
                {
                    Debug.LogWarning($"Card: {cards[i].name} has not been set an ID");
                }
            }
        }

        public CardTemplate GetCardById(int id)
        {
            var card = cards.Where(c => c.Id == id).FirstOrDefault();

            if(card == null)
            {
                Debug.LogError($"Attempting to get a card with an invalid id: {id}");
            }

            return card;
        }
    }
}
