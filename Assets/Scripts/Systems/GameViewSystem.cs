using CardGame.Common;
using CardGame.Common.StateMachines;
using CardGame.Factories;
using CardGame.GameStates;
using Photon.Pun;
using UnityEngine;

namespace CardGame.Systems
{
    public class GameViewSystem : MonoBehaviour, IAspect
    {
        private IContainer container = null;
        private ActionSystem actionSystem = null;

        public IContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = GameFactory.Create();
                    container.AddAspect(this);
                }
                return container;
            }
            set
            {
                container = value;
            }
        }

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
                return;
            }

            container.Awake();
            actionSystem = container.GetAspect<ActionSystem>();
        }

        private void Start() => container.ChangeState<PlayerIdleState>();

        private void Update() => actionSystem.Update();
    }
}
