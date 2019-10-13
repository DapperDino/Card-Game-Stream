namespace CardGame.Cards
{
    public class Hero : ICard, IArmoured, ICombatant, IDestructable
    {
        private readonly HeroTemplate template = null;

        public Hero(byte ownerIndex, HeroTemplate heroTemplate)
        {
            Health = heroTemplate.MaxHealth;
            Armour = heroTemplate.StartingArmour;

            template = heroTemplate;
            OwnerIndex = ownerIndex;
        }

        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int Armour { get; set; }
        public int Attack { get; }
        public int RemainingAttacks { get; set; }
        public int AllowedAttacks { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; }
        public byte OwnerIndex { get; }
    }
}
