using CardGame.Cards;
using System;
using System.Collections.Generic;

namespace CardGame.Models
{
    [Serializable]
    public class Player
    {
        public const int MaxDeck = 30;

        public Player(byte index)
        {
            Index = index;
        }

        public byte Index { get; }
        public Mana Mana { get; } = new Mana();
        public List<Card> Hero { get; } = new List<Card>();
        public List<Card> Deck { get; } = new List<Card>();
        public List<Card> Hand { get; } = new List<Card>();
        public List<Card> Battlefield { get; } = new List<Card>();
        public List<Card> Graveyard { get; } = new List<Card>();
    }
}
