using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum CameraType
{
    None,
    CameraFollow,
    CameraOrbit,
    CameraFixedFollow
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraType cameraType;
    [SerializeField] private Transform  cameraTransform;

    private CameraFollow      cameraFollow;
    private CameraOrbit       cameraOrbit;
    private CameraFixedFollow cameraFixedFollow;

    private CameraType previousType = CameraType.None;

    private void Start()
    {
        cameraFollow      = cameraTransform.GetComponent<CameraFollow>();
        cameraOrbit       = cameraTransform.GetComponent<CameraOrbit>();
        cameraFixedFollow = cameraTransform.GetComponent<CameraFixedFollow>();

        DisableCameras();
    }

    void Update()
    {
        if (previousType == cameraType)
        {
            return;
        }

        DisableCameras();

        switch (cameraType)
        {
            case CameraType.CameraFollow:
                cameraFollow.enabled = true;
                cameraFollow.Activate();
                break;
            case CameraType.CameraOrbit:
                cameraOrbit.enabled = true; 
                cameraOrbit.Activate();
                break;
            case CameraType.CameraFixedFollow:
                cameraFixedFollow.enabled = true;
                break;
        }

        previousType = cameraType;
    }

    private void DisableCameras()
    {
        cameraFollow.enabled = false;
        cameraOrbit.enabled = false;
        cameraFixedFollow.enabled = false;
    }
}
