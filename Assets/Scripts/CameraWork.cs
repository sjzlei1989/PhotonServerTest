using UnityEngine;
using System.Collections;
using System;

public class CameraWork : MonoBehaviour
{
    public float distance = 7.0f;
    public float height = 3.0f;
    public float heightSmoothLag = 0.3f;
    public Vector3 centerOffset = Vector3.zero;
    public bool followOnStart = false;

    Transform cameraTransform;
    bool isFollowing;
    private float heightVelocity = 0.0f;
    private float targetHeight = 10000.0f;

    void Start()
    {
        if(followOnStart) {
            OnStartFollowing();
        }
    }

    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        Cut();
    }

    private void Cut()
    {
        float oldHeightSmooth = heightSmoothLag;
        heightSmoothLag = 0.001f;
        Apply();
        heightSmoothLag = oldHeightSmooth;
    }

    void LateUpdate()
    {
        if(null == cameraTransform && isFollowing) {
            OnStartFollowing();
        }

        if(isFollowing) {
            Apply();
        }
    }

    private void Apply()
    {
        Vector3 targetCenter = transform.position + centerOffset;

        float originalTargetAngle = transform.eulerAngles.y;
        float currentAngle = cameraTransform.eulerAngles.y;

        float targetAngle = originalTargetAngle;

        currentAngle = targetAngle;

        targetHeight = targetCenter.y + height;

        float currentHeight = cameraTransform.position.y;
        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, heightSmoothLag);

        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        cameraTransform.position = targetCenter;
        cameraTransform.position += currentRotation * Vector3.back * distance;

        cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);

        SetUpRotation(targetCenter);
    }

    private void SetUpRotation(Vector3 _targetCenter)
    {
        Vector3 cameraPos = cameraTransform.position;
        Vector3 offsetToCenter = _targetCenter - cameraPos;

        Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));

        Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
        cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);
    }
}
