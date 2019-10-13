namespace CardGame.Cards
{
    public abstract class Card : ICard
    {
        protected CardTemplate template = null;

        public Card(byte ownerIndex)
        {
            OwnerIndex = ownerIndex;
        }

        public int Id => template.Id;
        public string Name => template.Name;
        public string Description => template.Description;
        public int ManaCost => template.ManaCost;
        public byte OwnerIndex { get; } = 0;
    }
}
