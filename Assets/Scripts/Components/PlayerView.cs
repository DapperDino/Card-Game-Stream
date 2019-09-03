using Photon.Pun;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardGame.Components
{
    public class PlayerView : MonoBehaviour
    {
        [Required] [SerializeField] private TextMeshProUGUI playerNameText = null;
        [Required] [SerializeField] private DeckView deckView = null;
        [Required] [SerializeField] private HandView handView = null;

        public void SetPlayerName(int viewIndex)
        {
            switch (viewIndex)
            {
                case 0:
                    playerNameText.text = PhotonNetwork.NickName;
                    break;

                case 1:
                    playerNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
                    break;

                default:
                    Debug.LogError("Incorrect number of players!");
                    break;
            }
        }
    }
}
