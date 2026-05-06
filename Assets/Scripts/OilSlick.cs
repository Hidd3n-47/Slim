using System;
using UnityEngine;

public class OilSlick : MonoBehaviour
{
    public float slipMultiplier = 3.0f;

    private Context multipliedContext = null;

    private void OnTriggerEnter(Collider other)
    {
        Context context = other.gameObject.GetComponentInParent<Context>();
        if (context)
        {
            //context.WheelsPower *= slipMultiplier;
            //todo what if there was already another context??
            multipliedContext = context;
            context.Forklift.ExternalForces.Add(new Force() { Transform = context.Forklift.transform, Type = ForceType.Translation, Power = slipMultiplier, Resistance = context.WheelsResistance });
        }


    }

    private void OnTriggerExit(Collider other)
    {
        //todo what if its not?
        //if (other.gameObject.GetComponentInParent<Context>() == multipliedContext && multipliedContext != null)
        //{
        //    multipliedContext.WheelsPower /= slipMultiplier;
        //    multipliedContext = null;
        //}
    }
}
