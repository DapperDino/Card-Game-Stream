namespace CardGame.Cards
{
    public interface ICombatant
    {
        int Attack { get; set; }
        int RemainingAttacks { get; set; }
        int AllowedAttacks { get; set; }
    }
}
