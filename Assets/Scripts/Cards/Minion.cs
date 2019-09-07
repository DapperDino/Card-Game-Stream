namespace CardGame.Cards
{
    public class Minion : Card, ICombatant, IDestructable
    {
        private int attackModifier = 0;
        private int maxHealthModifier = 0;

        public Minion(byte ownerIndex, MinionTemplate minionTemplate) : base(ownerIndex)
        {
            template = minionTemplate;
        }

        public int Attack
        {
            get => ((MinionTemplate)template).Attack + attackModifier;
            set => attackModifier += value;
        }

        public int RemainingAttacks { get; set; }

        public int AllowedAttacks { get; set; }

        public int Health { get; set; }

        public int MaxHealth
        {
            get => ((MinionTemplate)template).MaxHealth + maxHealthModifier;
            set => maxHealthModifier += value;
        }
    }
}
