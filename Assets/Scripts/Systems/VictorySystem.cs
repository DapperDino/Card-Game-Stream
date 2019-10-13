using CardGame.Common;
using CardGame.Models;
using CardGame.Cards;

namespace CardGame.Systems
{
    public class VictorySystem : Aspect
    {
        public bool IsGameOver()
        {
            var match = Container.GetMatch();

            foreach (Player player in match.Players)
            {
                var hero = player.Hero as Hero;

                if (hero.Health <= 0) { return true; }
            }

            return false;
        }
    }
}
