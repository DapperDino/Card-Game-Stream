namespace CardGame.Models
{
    public class Match
    {
        public const byte PlayerCount = 2;

        public Match()
        {
            for (byte i = 0; i < PlayerCount; i++)
            {
                Players[i] = new Player(i);
            }
        }

        public byte CurrentPlayerIndex { get; set; } = 0;
        public Player[] Players { get; } = new Player[PlayerCount];
        public Player CurrentPlayer => Players[CurrentPlayerIndex];
        public Player OpponentPlayer => Players[1 - CurrentPlayerIndex];
    }
}
