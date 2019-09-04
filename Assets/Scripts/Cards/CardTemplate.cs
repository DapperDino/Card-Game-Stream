using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
    public class CardTemplate : ScriptableObject
    {
        [SerializeField] private int id = -1;
        [SerializeField] private string title = "New Card Name";

        public int Id => id;
        public string Title => title;
    }
}
