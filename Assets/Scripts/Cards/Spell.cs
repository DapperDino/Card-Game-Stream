namespace CardGame.Cards
{
    public class Spell : Card
    {
        public Spell(byte ownerIndex, SpellTemplate spellTemplate) : base(ownerIndex)
        {
            template = spellTemplate;
        }
    }
}
