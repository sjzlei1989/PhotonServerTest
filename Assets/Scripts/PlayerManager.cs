#if UNITY_5 && (!UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && !UNITY_5_3) || UNITY_6
#define UNITY_MIN_5_4
#endif

using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : Photon.PunBehaviour, IPunObservable
{
    public GameObject beams;
    public GameObject playerUIPrefab;
    public static GameObject localPlayerInstance;
    bool isFiring;
    public float health = 1f;
    private void Awake()
    {
        if(photonView.isMine) {
            localPlayerInstance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
        if(null == beams) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else {
            beams.SetActive(false);
        }
    }

    private void Start()
    {
        CameraWork cameraWork = gameObject.GetComponent<CameraWork>();
        if(cameraWork != null) {
            if(photonView.isMine) {
                cameraWork.OnStartFollowing();
            }
        }
        else {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork on playerPrefab.", this);
        }

        if(playerUIPrefab != null) {
            GameObject uiGo = Instantiate(playerUIPrefab) as GameObject;
            uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else {
            Debug.Log("playerUIPrefab is null");
        }

#if UNITY_MIN_5_4
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        };
#endif
    }

    private void Update()
    {
        ProcessInputs();

        if(beams != null && isFiring != beams.GetActive()) {
            beams.SetActive(isFiring);
        }

        if(health <= 0f) {
            GameManager.instance.LeaveRoom();
        }
    }


    private void ProcessInputs()
    {
        if(Input.GetButtonDown("Fire1")) {
            if(!isFiring) {
                isFiring = true;
            }
            if(Input.GetButtonUp("Fire1")) {
                if(isFiring) {
                    isFiring = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!photonView.isMine) {
            return;
        }

        if(!other.name.Contains("Beam")) {
            return;
        }

        health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!photonView.isMine) {
            return;
        }

        if(!other.name.Contains("Beam")) {
            return;
        }

        health -= 0.1f * Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting) {
            stream.SendNext(isFiring);
            stream.SendNext(health);
        }
        else {
            this.isFiring = (bool)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
        }
    }

#if !UNITY_MIN_5_4
    void OnLevelWasLoaded(int level) {
        CalledOnLevelWasLoaded(level);
    }
#endif

    void CalledOnLevelWasLoaded(int level)
    {
        if(!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
            transform.position = new Vector3(0f, 5f, 0f);
        }

        GameObject uiGo = Instantiate(playerUIPrefab) as GameObject;
        uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
}
