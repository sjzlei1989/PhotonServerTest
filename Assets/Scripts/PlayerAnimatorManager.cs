using UnityEngine;
using System.Collections;

public class PlayerAnimatorManager : Photon.MonoBehaviour
{
    private Animator animator;
    public float directionDampTime = 0.25f;
	void Start ()
	{
        animator = gameObject.GetComponent<Animator>();
        if(!animator) {
            Debug.LogError("PlayerAnimatorManager is Missing Animator", this);
        }
	}

	void Update ()
	{
        if(false == photonView.isMine && true == PhotonNetwork.connected) {
            return;
        }

        if(!animator) {
            return;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Base Layer.Run")) {
            if(Input.GetButtonDown("Fire2")) {
                animator.SetTrigger("Jump");
            }
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(v < 0) {
            v = 0;
        }

        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
	}
}
