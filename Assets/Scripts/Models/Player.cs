using CardGame.Cards;
using System;
using System.Collections.Generic;

namespace CardGame.Models
{
    [Serializable]
    public class Player
    {
        public const int MaxDeck = 5;
        public const int MaxHand = 3;

        public Player(byte index)
        {
            Index = index;
        }

        public byte Index { get; } = 0;
        public int Fatigue { get; set; } = 0;
        public Mana Mana { get; } = new Mana();
        public Hero Hero { get; set; } = null;
        public List<Card> Deck { get; } = new List<Card>();
        public List<Card> Hand { get; } = new List<Card>();
        public List<Card> Battlefield { get; } = new List<Card>();
        public List<Card> Graveyard { get; } = new List<Card>();
    }
}
