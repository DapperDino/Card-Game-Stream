using CardGame.Models;
using CardGame.Systems;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Components
{
    public class BoardView : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Required] [SerializeField] private GameViewSystem gameViewSystem = null;
        [SerializeField] private PlayerView[] playerViews = new PlayerView[0];

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            photonView.RPC("SetPlayerNames", RpcTarget.All);
        }

        [PunRPC]
        private void SetPlayerNames()
        {
            for (int i = 0; i < playerViews.Length; i++)
            {
                playerViews[i].SetPlayerName(i);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}
