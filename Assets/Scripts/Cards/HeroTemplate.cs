using UnityEngine;

namespace CardGame.Cards
{
    [CreateAssetMenu(fileName = "New Hero Card", menuName = "Cards/Hero")]
    public class HeroTemplate : CardTemplate
    {
        public override Card CreateInstance(byte ownerIndex) => new Hero(ownerIndex, this);
    }
}