using CardGame.Common;
using CardGame.Common.StateMachines;
using CardGame.Systems;

namespace CardGame.Factories
{
    public class GameFactory
    {
        public static Container Create()
        {
            var game = new Container();

            game.AddAspect<ActionSystem>();

            game.AddAspect<StateMachine>();

            return game;
        }
    }
}
