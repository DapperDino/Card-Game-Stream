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
        private IContainer game = null;
        private ActionSystem actionSystem = null;

        public IContainer Game
        {
            get
            {
                if (game == null)
                {
                    game = GameFactory.Create();
                    game.AddAspect(this);
                }
                return game;
            }
            set
            {
                game = value;
            }
        }

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
                return;
            }

            Game.Awake();
            actionSystem = game.GetAspect<ActionSystem>();
        }

        private void Start() => Game.ChangeState<PlayerIdleState>();

        private void Update() => actionSystem.Update();
    }
}
