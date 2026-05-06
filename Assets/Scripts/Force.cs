using System.Diagnostics;
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
    public float Power = 0.0f;
    public float Resistance = 0.0f;
    public Vector3 Direction = Vector3.zero;

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

        //todo refactor to be ternary.
        if (Power > 0)
        {
            Power -= Resistance;

            // todo max function instead
            if (Power < 0)
            {
                Power = 0;
            }
        }
        else
        {
            Power += Resistance;

            // todo min function instead.
            if (Power > 0)
            {
                Power = 0;
            }
        }
    }
}
