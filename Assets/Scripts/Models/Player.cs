using CardGame.Cards;
using System;
using System.Collections.Generic;

namespace CardGame.Models
{
    [Serializable]
    public class Player
    {
        public Player(int index)
        {
            Index = index;
        }

        public int Index { get; }
        public List<Card> Deck { get; } = new List<Card>();
    }
}
