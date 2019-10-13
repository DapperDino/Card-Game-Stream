using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Hero Card", menuName = "Cards/Hero")]
    public class HeroTemplate : ScriptableObject
    {
        [Header("Hero Data")]
        [SerializeField] private int id = -1;
        [SerializeField] private new string name = "New Card Name";
        [SerializeField] private string description = "New Card Description";
        [SerializeField] private int maxHealth = 30;
        [SerializeField] private int startingArmour = 0;

        public int Id => id;
        public string Name => name;
        public string Description => description;
        public int MaxHealth => maxHealth;
        public int StartingArmour => startingArmour;

        public Hero CreateInstance(byte ownerIndex) => new Hero(ownerIndex, this);
    }
}