namespace CardGame.Cards
{
    public interface ICard
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }      
        byte OwnerIndex { get; }
    }
}
