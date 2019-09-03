using CardGame.Models;
using Photon.Pun;
using UnityEngine;

namespace CardGame.Systems
{
    public class GameViewSystem : MonoBehaviour
    {
        public Match Match { get; private set; } = null;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            Match = new Match();
        }
    }
}
