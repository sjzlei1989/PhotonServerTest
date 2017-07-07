using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    public void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if(!PhotonNetwork.isMasterClient) {
            Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master.");
        }

        Debug.Log("PhotonNetwork: Loading Level: " + PhotonNetwork.room.PlayerCount);
        PhotonNetwork.LoadLevel("Room" + PhotonNetwork.room.PlayerCount);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected: A new Player Connected.");
        if(PhotonNetwork.isMasterClient) {
            Debug.Log("OnPhotonPlayerconnected: is masterc lient, LoadArena()");
            LoadArena();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerDisconnected: A player left.");

        if(PhotonNetwork.isMasterClient) {
            Debug.Log("OnPhotonPlayerDisconnected: is master Client, LoadArena()");
            LoadArena();
        }
    }
}
