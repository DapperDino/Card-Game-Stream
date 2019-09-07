namespace CardGame.Cards
{
    public interface ICombatant
    {
        int Attack { get; }
        int RemainingAttacks { get; set; }
        int AllowedAttacks { get; set; }
    }
}
