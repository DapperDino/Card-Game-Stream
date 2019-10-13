using CardGame.Common.StateMachines;
using UnityEngine;

namespace CardGame.GameStates
{
    public class GameOverState : BaseState
    {
        public override void Enter() => Debug.Log("Game Over");
    }
}
