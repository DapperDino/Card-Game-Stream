using CardGame.Cards;
using CardGame.Models;
using System.Collections.Generic;

namespace CardGame.GameActions
{
    public class DrawCardsAction : GameAction
    {
        public DrawCardsAction(Player player, int amount)
        {
            Player = player;
            Amount = amount;
        }

        public List<Card> Cards { get; set; } = null;
        public int Amount { get; } = 0;
    }
}
