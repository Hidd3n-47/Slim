using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum CameraType
{
    None,
    CameraFollow,
    CameraOrbit,
    CameraFixedFollow,
    CameraFreeOrbit
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraType cameraType;
    [SerializeField] private Transform  cameraTransform;

    [SerializeField] private Transform orbitCameraPosition;
    [SerializeField] private Transform freeOrbitCameraPosition;
    [SerializeField] private Transform fixedFollowCameraPosition;
    [SerializeField] private Transform followCameraPosition;

    private CameraFollow      cameraFollow;
    private CameraOrbit       cameraOrbit;
    private CameraFreeOrbit   cameraFreeOrbit;
    private CameraFixedFollow cameraFixedFollow;

    private CameraType previousType = CameraType.None;

    private void Start()
    {
        orbitCameraPosition.gameObject.SetActive(false);
        fixedFollowCameraPosition.gameObject.SetActive(false);
        followCameraPosition.gameObject.SetActive(false);
        freeOrbitCameraPosition.gameObject.SetActive(false);

        cameraFollow      = cameraTransform.GetComponent<CameraFollow>();
        cameraOrbit       = cameraTransform.GetComponent<CameraOrbit>();
        cameraFreeOrbit   = cameraTransform.GetComponent<CameraFreeOrbit>();
        cameraFixedFollow = cameraTransform.GetComponent<CameraFixedFollow>();

        DisableCameras();
    }

    private void Update()
    {
        if (previousType == cameraType)
        {
            return;
        }

        DisableCameras();

        switch (cameraType)
        {
            case CameraType.CameraFollow:
                cameraTransform.localPosition = followCameraPosition.localPosition;
                cameraTransform.localRotation = followCameraPosition.localRotation;
                cameraFollow.enabled = true;
                cameraFollow.Activate();
                break;
            case CameraType.CameraOrbit:
                cameraTransform.localPosition = orbitCameraPosition.localPosition;
                cameraTransform.localRotation = orbitCameraPosition.localRotation;
                cameraOrbit.enabled = true; 
                cameraOrbit.Activate();
                break;
            case CameraType.CameraFixedFollow:
                cameraTransform.localPosition = fixedFollowCameraPosition.localPosition;
                cameraTransform.localRotation = fixedFollowCameraPosition.localRotation;
                cameraFixedFollow.enabled = true;
                break;
            case CameraType.CameraFreeOrbit:
                cameraTransform.localPosition = freeOrbitCameraPosition.localPosition;
                cameraTransform.localRotation = freeOrbitCameraPosition.localRotation;
                cameraFreeOrbit.enabled = true;
                cameraFreeOrbit.Activate();
                break;
        }

        previousType = cameraType;
    }

    private void DisableCameras()
    {
        cameraFollow.enabled      = false;
        cameraOrbit.enabled       = false;
        cameraFixedFollow.enabled = false;
        cameraFreeOrbit.enabled   = false;
    }
}
