using CardGame.Cards;
using System.Collections.Generic;

namespace CardGame.GameActions
{
    public class DamageAction : GameAction
    {
        public List<IDestructable> Targets { get; } = null;
        public int Amount { get; } = 0;

        public DamageAction(IDestructable target, int amount)
        {
            Targets = new List<IDestructable>(1) { target };
            Amount = amount;
        }

        public DamageAction(List<IDestructable> targets, int amount)
        {
            Targets = targets;
            Amount = amount;
        }
    }
}
