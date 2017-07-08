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
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    public void Connect()
    {
//         isConnected = true;
//         if(PhotonNetwork.connected) {
//             PhotonNetwork.JoinRandomRoom();
//         }
//         else {
//             PhotonNetwork.ConnectUsingSettings(gameVersion);
//         }
//         controlPanel.SetActive(false);
//         progressLabel.SetActive(true);
    }



    #region PunBehaviour Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
        isConnected = true;
    }

    private void OnGUI()
    {
        if(isConnected) {
            if(GUI.Button(new Rect(100, 100, 100, 30), "GetRoomList")) {
                RoomInfo[] roomList = PhotonNetwork.GetRoomList();
                if(0 == roomList.Length) {
                    Debug.Log("当前大厅没有房间, 现在创建一个");
                    PhotonNetwork.CreateRoom("test", new RoomOptions() { MaxPlayers = 4 }, new TypedLobby());
                }
                else {
                    for(int i = 0; i < roomList.Length; i++) {
                        Debug.Log(roomList[i].ToStringFull());
                    }
                }
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        progressLabel.SetActive(false);
        Debug.Log(PhotonNetwork.lobby.ToString());
        isConnected = true;
//         RoomInfo[] roomList = PhotonNetwork.GetRoomList();
//         if(0 == roomList.Length) {
//             Debug.Log("当前大厅没有房间, 现在创建一个");
//             PhotonNetwork.CreateRoom("test", new RoomOptions() { MaxPlayers = 4 }, new TypedLobby());
//         }
//         else {
//             for(int i = 0; i < roomList.Length; i++) {
//                 Debug.Log(roomList[i].ToStringFull());
//             }
//         }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.LoadLevel("Room" + PhotonNetwork.room.PlayerCount);
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        for(int i = 0; i < roomList.Length; i++) {
            Debug.Log(roomList[i].ToStringFull());
        }
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        Debug.Log("Create a new room.");
        PhotonNetwork.CreateRoom("test", new RoomOptions() { MaxPlayers = 4 }, null);
    }
    #endregion
}
