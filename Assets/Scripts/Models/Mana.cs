using UnityEngine;

namespace CardGame.Models
{
    public class Mana
    {
        public const int MaxSlots = 10;

        private int spent = 0;
        private int permanent = 0;

        public int Unlocked => Mathf.Min(permanent, MaxSlots);
        public int Available => Mathf.Min(permanent - spent, MaxSlots);
    }
}
