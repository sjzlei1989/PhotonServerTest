using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    static public GameManager instance;
    public GameObject playerPrefab;
    private void Start()
    {
        instance = this;
        if(null == playerPrefab) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Referentce. ", this);
        }
        else {
            if(null == PlayerManager.localPlayerInstance) {
                Debug.Log("We are instantiating localplayer from " + SceneManager.GetActiveScene().name);
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else {
                Debug.Log("Ignoring scene load for " + SceneManager.GetActiveScene().name);
            }
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
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
