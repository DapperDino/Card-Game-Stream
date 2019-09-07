using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Minion Card", menuName = "Cards/Minion")]
    public class MinionTemplate : CardTemplate
    {
        [Header("Minion Data")]
        [SerializeField] private int attack = 0;
        [SerializeField] private int maxHealth = 0;

        public int Attack => attack;
        public int MaxHealth => maxHealth;

        public override Card CreateInstance(byte ownerIndex) => new Minion(ownerIndex, this);
    }
}
