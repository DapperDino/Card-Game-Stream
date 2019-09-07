using Photon.Pun;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardGame.Views
{
    public class BoardView : MonoBehaviourPunCallbacks
    {
        [Required] [SerializeField] TextMeshProUGUI allyNameText = null;
        [Required] [SerializeField] TextMeshProUGUI opponentNameText = null;
        [SerializeField] private PlayerView[] playerViews = new PlayerView[0];

        public PlayerView[] PlayerViews => playerViews;

        private void Start()
        {
            allyNameText.text = PhotonNetwork.LocalPlayer.NickName;
            opponentNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
        }
    }
}
