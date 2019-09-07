using System;

namespace CardGame.Cards
{
    [Flags]
    public enum Zones
    {
        None = 0,
        Hero = 1 << 0,
        Deck = 1 << 1,
        Hand = 1 << 2,
        Battlefield = 1 << 3,
        Graveyard = 1 << 4,
        Active = Hero | Battlefield,
    }

    public static class ZonesExtensions
    {
        public static bool Contains(this Zones source, Zones target) => (source & target) == target;
    }
}