namespace CardGame.Models
{
    public class Match
    {
        public const int PlayerCount = 2;

        private int currentPlayerIndex = 0;

        public Match()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                Players[i] = new Player(i);
            }
        }

        public Player[] Players { get; } = new Player[PlayerCount];
        public Player CurrentPlayer => Players[currentPlayerIndex];
        public Player OpponentPlayer => Players[1 - currentPlayerIndex];
    }
}
