using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject nameInputScreen;
    public TMP_InputField nameInput;
    private bool hasSetNick;

    public GameObject startGameButton;
    public static Launcher instance;
    public GameObject loadingScreen;
    public GameObject createRoomScreen;
    public TMP_Text loadingtext;
    public TMP_InputField roomNameInput;
    public GameObject roomScreen;
    public TMP_Text roomNameText;
    public GameObject roomBrowserScreen;
    public RoomBehaviors roomButton;
    private List<RoomBehaviors> allRoomButtons = new List<RoomBehaviors>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        loadingScreen.SetActive(true);
        loadingtext.text = "Connecting to server...";

        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingtext.text = "Connected, Joining Lobby...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        loadingtext.text = "Joined Lobby";
        loadingScreen.SetActive(false);
        if (!hasSetNick)
        {
            nameInputScreen.SetActive(true);

            if (PlayerPrefs.HasKey("PlayerName"))
            {
                nameInput.text = PlayerPrefs.GetString("PlayerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
        }
    }
    public void openCreateRoom()
    {
        createRoomScreen.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        loadingtext.text = "Failed to create room";
        loadingScreen.SetActive(false);
    }
    public override void OnJoinedRoom()
    {
        loadingtext.text = "Joined Room";
        //PhotonNetwork.LoadLevel("Game");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        loadingtext.text = "Player Joined";
        //PhotonNetwork.LoadLevel("Game");
    }
    public override void OnCreatedRoom()
    {
        roomScreen.SetActive(true);
        loadingtext.text = "Room Created";
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        //PhotonNetwork.LoadLevel("Game");
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPlayerLabel.text = newPlayer.NickName;
        newPlayerLabel.gameObject.SetActive(true);

        playerList.Add(newPlayerLabel);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListAllPlayers();
    }
    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomScreen.SetActive(false);
        loadingtext.text = "Leaving Room";
        loadingScreen.SetActive(true);
    }
    public void createRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInput.text, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
        createRoomScreen.SetActive(false);
        loadingtext.text = "Creating Room...";
        loadingScreen.SetActive(true);
    }
    public void quitButton()
    {
        Application.Quit();
    }
        public void OpenRoomBrowser()   
    {
        roomBrowserScreen.SetActive(true);
    }

    public void CloseRoomBrowser()
    {
        roomBrowserScreen.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomBehaviors button in allRoomButtons)
        {
            Destroy(button.gameObject);
        }

        allRoomButtons.Clear();

        roomButton.gameObject.SetActive(false);

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomBehaviors newButton = Instantiate(roomButton, roomButton.transform.parent);
                newButton.SetButtonDetails(roomList[i]);
                newButton.gameObject.SetActive(true);

                allRoomButtons.Add(newButton);
            } 
        }
    }
    public void SetNickName()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            return;
        }

        PlayerPrefs.SetString("PlayerName", nameInput.text);

        PhotonNetwork.NickName = nameInput.text;
        nameInputScreen.SetActive(false);
        hasSetNick = true;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }
}
