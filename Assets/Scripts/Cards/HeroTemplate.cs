using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Hero Card", menuName = "Cards/Hero")]
    public class HeroTemplate : CardTemplate
    {
        [SerializeField] private int maxHealth = 30;
        [SerializeField] private int startingArmour = 0;

        public int MaxHealth => maxHealth;
        public int StartingArmour => startingArmour;

        public override Card CreateInstance(byte ownerIndex) => new Hero(ownerIndex, this);
    }
}