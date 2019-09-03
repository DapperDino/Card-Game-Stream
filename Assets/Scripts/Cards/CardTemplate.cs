using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
    public class CardTemplate : ScriptableObject
    {
        [SerializeField] private int id = -1;
        [SerializeField] private new string name = "New Card Name";

        public int Id => id;
        public string Name => name;    
    }
}
