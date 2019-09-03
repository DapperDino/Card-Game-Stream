using Photon.Pun;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Menus.Main
{
    public class PlayerNameInput : MonoBehaviour
    {
        [Required] [SerializeField] private TMP_InputField nameInputField = null;
        [Required] [SerializeField] private Button continueButton = null;

        private const string PlayerPrefNameKey = "PlayerName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefNameKey)) { return; }

            string defaultName = PlayerPrefs.GetString(PlayerPrefNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;

            PhotonNetwork.NickName = playerName;

            PlayerPrefs.SetString(PlayerPrefNameKey, playerName);
        }
    }
}
