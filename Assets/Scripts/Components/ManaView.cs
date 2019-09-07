using CardGame.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Components
{
    public class ManaView : MonoBehaviour
    {
        [SerializeField] private Image[] slots = new Image[0];
        [Required] [SerializeField] private Sprite available = null;
        [Required] [SerializeField] private Sprite unavailable = null;
        [Required] [SerializeField] private Sprite slot = null;

        private void OnManaValueChanged(object sender, object args)
        {
            var mana = args as Mana;

            for (int i = 0; i < mana.Available; i++)
            {
                SetSpriteForImageSlot(available, i);
            }
            for(int i = mana.Available; i < mana.Unlocked; i++)
            {
                SetSpriteForImageSlot(unavailable, i);
            }
            for (int i = mana.Unlocked; i < Mana.MaxSlots; i++)
            {
                SetSpriteForImageSlot(slot, i);
            }
        }

        private void SetSpriteForImageSlot(Sprite sprite, int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < slots.Length)
            {
                slots[slotIndex].sprite = sprite;
            }
        }
    }
}
