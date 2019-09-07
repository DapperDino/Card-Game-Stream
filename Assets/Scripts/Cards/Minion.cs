namespace CardGame.Cards
{
    public class Minion : Card, ICombatant, IDestructable
    {
        public Minion(byte ownerIndex, MinionTemplate minionTemplate) : base(ownerIndex)
        {
            template = minionTemplate;
            Health = minionTemplate.MaxHealth;
        }

        public int Attack => ((MinionTemplate)template).Attack;
        public int RemainingAttacks { get; set; }
        public int AllowedAttacks { get; set; }

        public int Health { get; set; }
        public int MaxHealth => ((MinionTemplate)template).MaxHealth;
    }
}
