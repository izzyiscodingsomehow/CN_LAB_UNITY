using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    public GameObject loadingScreen;
    public GameObject createRoomScreen;
    public TMP_Text loadingtext;
    public TMP_InputField roomNameInput;
    public GameObject roomScreen;
    public TMP_Text roomNameText;
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
        loadingtext.text = "Connected, Joining Lobby...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        loadingtext.text = "Joined Lobby";
        loadingScreen.SetActive(false);
    }
    public void openCreateRoom()
    {
        createRoomScreen.SetActive(true);
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
}
