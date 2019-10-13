using CardGame.Common.Extensions;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame.Menus.Main
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [Required] [SerializeField] private GameObject findOpponentPanel = null;
        [Required] [SerializeField] private GameObject waitingStatusPanel = null;
        [Required] [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MaxPlayersPerRoom = 2;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void FindOpponent()
        {
            isConnecting = true;

            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);

            Debug.LogWarning($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No clients are waiting for an opponent, creating a new room");

            var roomOptions = new RoomOptions
            {
                EmptyRoomTtl = 1000 * 60,
                PlayerTtl = 1000 * 60,
                MaxPlayers = MaxPlayersPerRoom,
            };

            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if (playerCount != MaxPlayersPerRoom)
            {
                waitingStatusText.text = "Waiting For Opponent...";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                Debug.Log("Match is ready to start");
                waitingStatusText.text = "Opponent Found";
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                Debug.Log("Match is ready to start");
                waitingStatusText.text = "Opponent Found";

                List<byte> availableIndices = new List<byte>() { 0, 1 };

                for (int i = 1; i < 3; i++)
                {
                    byte chosenIndex = availableIndices.Random();
                    availableIndices.Remove(chosenIndex);
                    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "HeroIndex", 0 },
                        { "TurnIndex", chosenIndex }
                    };
                    PhotonNetwork.CurrentRoom.Players[i].SetCustomProperties(playerProperties);
                }

                PhotonNetwork.LoadLevel("Scene_Main");
            }
        }
    }
}
