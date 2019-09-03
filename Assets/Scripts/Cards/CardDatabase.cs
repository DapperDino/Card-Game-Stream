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
        }
    }
}
