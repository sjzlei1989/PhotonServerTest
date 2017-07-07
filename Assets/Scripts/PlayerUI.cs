using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text playerNameText;
    public Slider playerHealthSlider;
    PlayerManager target;
    public Vector3 screenOffset = new Vector3(0f, 30f, 0f);
    float characterControllerHeight = 0f;
    Transform targetTransform;
    Vector3 targetPosition;

    private void Awake()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void SetTarget(PlayerManager _target)
    {
        if(null == _target) {
            Debug.Log("target is null");
            return;
        }
        target = _target;
        if(playerNameText != null) {
            playerNameText.text = target.photonView.owner.NickName;
        }
        CharacterController cc = target.GetComponent<CharacterController>();
        if(cc != null) {
            characterControllerHeight = cc.height;
        }
        targetTransform = target.transform;
    }

    private void Update()
    {
        if(null == target) {
            Destroy(gameObject);
            return;
        }

        if(playerHealthSlider != null) {
            playerHealthSlider.value = target.health;
        }
    }

    private void LateUpdate()
    {
        if(targetTransform != null) {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
}
