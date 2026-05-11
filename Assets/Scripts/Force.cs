using System;
using UnityEngine;

public enum ForceType
{
    Translation,
    Rotation
}

public class Force
{
    public ForceType Type = ForceType.Translation;

    public Transform Transform = null;
    public float Power         = 0.0f;
    public float Resistance    = 0.0f;
    public Vector3 Direction   = Vector3.zero;

    public bool PreventResistanceForOneFrame = false;

    public void Refresh()
    {
        switch (Type)
        {
            case ForceType.Translation:
                Transform.Translate(Direction * Power * Time.deltaTime, Space.World);
                break;
            case ForceType.Rotation:
                Transform.Rotate(Direction * Power * Time.deltaTime, Space.World);
                break;
        }

        if (PreventResistanceForOneFrame)
        {
            PreventResistanceForOneFrame = false;
            return;
        }

        int sign = Math.Sign(Power);

        // Since Resistance is opposing the force, multiply by -1.
        Power += Resistance * (-1 * sign) * Time.deltaTime;
        // If the force changes direction, reset back to zero.
        Power = sign > 0 ? Math.Max(Power, 0) : Math.Min(Power, 0);
    }
}
