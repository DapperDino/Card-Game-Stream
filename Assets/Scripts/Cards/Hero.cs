namespace CardGame.Cards
{
    public class Hero : Card, IArmoured, ICombatant, IDestructable
    {
        public Hero(byte ownerIndex, HeroTemplate heroTemplate) : base(ownerIndex)
        {
            Health = heroTemplate.MaxHealth;
            Armour = heroTemplate.StartingArmour;

            template = heroTemplate;
        }

        public int Armour { get; set; }
        public int Attack { get; }
        public int RemainingAttacks { get; set; }
        public int AllowedAttacks { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; }
    }
}
