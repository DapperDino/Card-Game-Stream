using CardGame.Cards;
using CardGame.Common;
using CardGame.GameActions;
using UnityEngine;

namespace CardGame.Systems
{
    public class DestructableSystem : Aspect, IObserve
    {
        public void Awake()
        {
            throw new System.NotImplementedException();
        }

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        private void OnPerformDamageAction(object sender, object args)
        {
            var damageAction = args as DamageAction;
            foreach (IDestructable target in damageAction.Targets)
            {
                target.Health -= damageAction.Amount;
            }
        }
    }
}
