using System;

namespace CardGame.Cards
{
    [Serializable]
    public class Card
    {
        private CardTemplate template = null;

        public Card(CardTemplate template)
        {
            this.template = template;
        }
    }
}
