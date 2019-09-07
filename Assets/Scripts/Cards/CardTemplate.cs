using UnityEngine;

namespace CardGame.Cards
{
    public abstract class CardTemplate : ScriptableObject
    {
        [Header("Base Card Data")]
        [SerializeField] private int id = -1;
        [SerializeField] private new string name = "New Card Name";
        [SerializeField] private string description = "New Card Description";
        [SerializeField] private int manaCost = 0;

        public int Id => id;
        public string Name => name;
        public string Description => description;
        public int ManaCost => manaCost;

        public abstract Card CreateInstance(byte ownerIndex);
    }
}
