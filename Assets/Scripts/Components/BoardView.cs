using Photon.Pun;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardGame.Components
{
    public class BoardView : MonoBehaviourPunCallbacks
    {
        [Required] [SerializeField] TextMeshProUGUI allyNameText = null;
        [Required] [SerializeField] TextMeshProUGUI opponentNameText = null;

        private void Start()
        {
            allyNameText.text = PhotonNetwork.LocalPlayer.NickName;
            opponentNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
        }
    }
}
