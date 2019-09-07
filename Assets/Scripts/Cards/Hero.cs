namespace CardGame.Cards
{
    public class Hero : Card, IArmoured, ICombatant, IDestructable
    {
        public Hero(byte ownerIndex, HeroTemplate heroTemplate) : base(ownerIndex)
        {
            template = heroTemplate;
        }

        public int Armour { get; set; }
        public int Attack { get; set; }
        public int RemainingAttacks { get; set; }
        public int AllowedAttacks { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
}
