using CardGame.Common;
using CardGame.Models;

namespace CardGame.Systems
{
    public class DataSystem : Aspect
    {
        public Match Match { get; } = new Match();
    }

    public static class DataSystemExtensions
    {
        public static Match GetMatch(this IContainer container)
        {
            var dataSystem = container.GetAspect<DataSystem>();
            return dataSystem.Match;
        }
    }
}
