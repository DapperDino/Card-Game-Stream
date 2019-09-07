using CardGame.Models;

namespace CardGame.GameActions
{
    public class OverdrawAction : DrawCardsAction
    {
        public OverdrawAction(Player player, int amount) : base(player, amount) { }
    }
}
