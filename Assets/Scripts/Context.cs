using System;
using Unity.VisualScripting;
using UnityEngine;

public class Context : MonoBehaviour
{
    public Forklift Forklift;
    public Camera Camera;
    //public CameraController camController;

    public float Brake = 0.3f;
    public float EngineAcceleration = 0.2f;
    public float EngineResistance = 0.1f;
    public float EnginePowerMax = 5.0f;
    public float EnginePowerMin = -3.0f;

    public float WheelsPower = 1.0f;
    public float WheelsPowerMax = 45.0f;
    public float WheelsPowerMin = -45.0f;
    public float WheelsResistance = 1.0f;

    private void Awake()
    {
        // camera parenting to current transform.
        Forklift.Engine.Resistance = EngineResistance;
        Forklift.Wheels.Resistance = WheelsResistance;

        Forklift.OnCollideFront += (Collision collision) =>
        {
            Forklift.Engine.Power = -Forklift.Engine.Power / 2.0f;
            Forklift.ExternalForces.Add(new Force()
            {
                Transform = Forklift.transform, Power = Forklift.Engine.Power, Resistance = Forklift.Engine.Resistance
            });
            // add feel or particles for the bump.
        };
        Forklift.OnCollideBack += (Collision collision) =>
        {
            Forklift.Engine.Power = -Forklift.Engine.Power / 2.0f;
            Forklift.ExternalForces.Add(new Force()
            {
                Transform = Forklift.transform,
                Power = Forklift.Engine.Power,
                Resistance = Forklift.Engine.Resistance
            });
            // add feel or particles for the bump.
        };
        Forklift.OnCollideLeft += (Collision collision) =>
        {
            Forklift.Engine.Power = -Forklift.Engine.Power / 2.0f;
            Forklift.ExternalForces.Add(new Force()
            {
                Transform = Forklift.transform,
                Power = Forklift.Engine.Power,
                Resistance = Forklift.Engine.Resistance
            });
            // add feel or particles for the bump.
        };
        Forklift.OnCollideRight += (Collision collision) =>
        {
            Forklift.Engine.Power = -Forklift.Engine.Power / 2.0f;
            Forklift.ExternalForces.Add(new Force()
            {
                Transform = Forklift.transform,
                Power = Forklift.Engine.Power,
                Resistance = Forklift.Engine.Resistance
            });
            // add feel or particles for the bump.
        };
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.W))
        {
            // if we are currently adding feel remove feel.
        }
        else
        {
            if (Forklift.Engine.Power < 0)
            {
                Forklift.Engine.Power += Brake;
                // ???? should this be engine power min??
                Forklift.Engine.Power = Forklift.Engine.Power > 0 ? 0 : Forklift.Engine.Power;
            }
            else
            {
                // if no feel, start feel.

                Forklift.Engine.PreventResistanceForOneFrame = true;
                Forklift.Engine.Power += EngineAcceleration;
                // ????
                Forklift.Engine.Power = Forklift.Engine.Power > EnginePowerMax ? EnginePowerMax : Forklift.Engine.Power;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (Forklift.Engine.Power > 0)
            {
                Forklift.Engine.Power -= Brake;
                //todo refactor.
                Forklift.Engine.Power = Forklift.Engine.Power < 0.0f ? 0.0f : Forklift.Engine.Power;
            }
            else
            {
                Forklift.Engine.PreventResistanceForOneFrame = true;
                Forklift.Engine.Power -= EngineAcceleration;
                Forklift.Engine.Power = Forklift.Engine.Power < EnginePowerMin ? EnginePowerMin : Forklift.Engine.Power;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            Forklift.Wheels.PreventResistanceForOneFrame = true;
            Forklift.Wheels.Power += WheelsPower;
            Forklift.Wheels.Power *= Forklift.Engine.Power != 0.0f ? 1.0f : 0.0f;
            Forklift.Wheels.Power =
                Forklift.Wheels.Direction.y > WheelsPowerMax ? WheelsPowerMax : Forklift.Wheels.Power;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Forklift.Wheels.PreventResistanceForOneFrame = true;
            Forklift.Wheels.Power -= WheelsPower;
            Forklift.Wheels.Power *= Forklift.Engine.Power != 0.0f ? 1.0f : 0.0f;
            Forklift.Wheels.Power =
                Forklift.Wheels.Direction.y < WheelsPowerMin ? WheelsPowerMin : Forklift.Wheels.Power;
        }
    }
}
