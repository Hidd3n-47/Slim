using System;
using UnityEngine;

[Serializable]
public class SurfaceInfo
{
    public float enginePowerModifier = 1.0f;
    public float turningSlipModifier = 1.0f;
}

public class GroundSurface : MonoBehaviour
{
    [SerializeField] private SurfaceInfo info;

    public SurfaceInfo Info => info;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("GroundSurface");
    }
}
