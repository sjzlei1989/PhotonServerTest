using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class Launcher : PunBehaviour
{
    public GameObject controlPanel;
    public GameObject progressLabel;

    public string gameVersion = "1";
    bool isConnected = false;
	void Start ()
	{
        progressLabel.SetActive(false);
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
        //Connect();
	}

    public override void OnConnectedToMaster()
    {
        if(isConnected) {
            Debug.Log("OnConnectedToMaster()");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void Connect()
    {
        isConnected = true;
        if(PhotonNetwork.connected) {
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.LoadLevel("Room" + PhotonNetwork.room.PlayerCount);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        Debug.Log("Create a new room.");
        PhotonNetwork.CreateRoom("test", new RoomOptions() { MaxPlayers = 4 }, null);
    }
}
