using CardGame.Cards;
using CardGame.GameActions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Views
{
    public class HeroView : MonoBehaviour
    {
        [Required] [SerializeField] private Image heroImage = null;
        [Required] [SerializeField] private TextMeshProUGUI healthText = null;
        [Required] [SerializeField] private TextMeshProUGUI attackText = null;
        [Required] [SerializeField] private TextMeshProUGUI armourText = null;
        [Required] [SerializeField] private Sprite active = null;
        [Required] [SerializeField] private Sprite inactive = null;

        public Hero Hero { get; private set; }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        public void SetHero(Hero hero)
        {
            Hero = hero;
            Refresh();
        }

        private void Refresh()
        {
            if (Hero == null) { return; }

            heroImage.sprite = inactive;
            attackText.text = Hero.Attack.ToString();
            healthText.text = Hero.Health.ToString();
            armourText.text = Hero.Armour.ToString();
        }

        private void OnPerformDamageAction(object sender, object args)
        {
            var damageAction = args as DamageAction;

            if (damageAction.Targets.Contains(Hero)) { Refresh(); }
        }
    }
}
