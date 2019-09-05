using CardGame.Cards;
using System;
using System.Collections.Generic;

namespace CardGame.Models
{
    [Serializable]
    public class Player
    {
        public Player(byte index)
        {
            Index = index;
        }

        public byte Index { get; }
        public List<Card> Deck { get; } = new List<Card>();
    }
}
