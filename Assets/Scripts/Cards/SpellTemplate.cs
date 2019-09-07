using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Spell Card", menuName = "Cards/Spell")]
    public class SpellTemplate : CardTemplate
    {
        public override Card CreateInstance(byte ownerIndex) => new Spell(ownerIndex, this);
    }
}
