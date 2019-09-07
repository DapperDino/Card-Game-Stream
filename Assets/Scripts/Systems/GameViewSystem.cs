using System.Collections.Generic;
using CardGame.Cards;
using CardGame.Common;
using CardGame.Common.StateMachines;
using CardGame.Factories;
using CardGame.GameStates;
using CardGame.Models;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Systems
{
    public class GameViewSystem : MonoBehaviour, IAspect
    {
        [Required] [SerializeField] private MinionTemplate testMinion = null;

        private IContainer game = null;
        private ActionSystem actionSystem = null;

        public IContainer Container
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

            Container.Awake();
            actionSystem = game.GetAspect<ActionSystem>();
            SetUp();
        }

        private void Start() => Container.ChangeState<PlayerIdleState>();

        private void Update() => actionSystem.Update();

        private void SetUp()
        {
            var match = Container.GetMatch();

            for (byte i = 0; i < match.Players.Length; i++)
            {
                var cards = new List<Card>();
                for (int j = 0; j < Player.MaxDeck; j++)
                {
                    cards.Add(testMinion.CreateInstance(i));
                }
                match.Players[i].Deck.AddRange(cards);
            }
        }
    }
}
