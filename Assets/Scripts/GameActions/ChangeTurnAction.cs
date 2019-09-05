namespace CardGame.GameActions
{
    public class ChangeTurnAction : GameAction
    {
        public byte TargetPlayerIndex { get; } = 0;

        public ChangeTurnAction(byte targetPlayerIndex)
        {
            TargetPlayerIndex = targetPlayerIndex;
        }
    }
}
