using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

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

    [SerializeField] private LayerMask surfaceLayerMask;
    private SurfaceInfo surfaceInfo = new SurfaceInfo();
    public Transform groundChecker;

    public Transform leftWheel;
    public Transform rightWheel;
    public float rotationAmount = 45.0f;
    private bool rotatedLeft = false;
    private bool rotatedRight = false;

    private void Awake()
    {
        //surfaceLayerMask = LayerMask.NameToLayer("GroundSurface");

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
    void DetectSurface()
    {
        SurfaceInfo info = new SurfaceInfo();
        if (Physics.Raycast(groundChecker.position, Vector3.down, out RaycastHit hit, 2.0f, surfaceLayerMask))
        {
            GroundSurface surf = hit.collider.GetComponent<GroundSurface>();
            info = surf?.Info;
        }

        surfaceInfo = info;
    }

    private void Update()
    {
        DetectSurface();

        float acceleration = Gamepad.current.rightTrigger.ReadValue();
        float brake = Gamepad.current.leftTrigger.ReadValue();

        float horizontalInput = (float)Math.Round(Gamepad.current.leftStick.value.x, 2);

        if (acceleration == 0.0f)
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
                Forklift.Engine.Power += EngineAcceleration * acceleration;
                Forklift.Engine.Power *= surfaceInfo.enginePowerModifier;
                // ????
                Forklift.Engine.Power = Forklift.Engine.Power > EnginePowerMax ? EnginePowerMax : Forklift.Engine.Power;
            }
        }

        if (brake != 0.0f)
        {
            if (Forklift.Engine.Power > 0)
            {
                Forklift.Engine.Power -= Brake * brake;
                //todo refactor.
                Forklift.Engine.Power = Forklift.Engine.Power < 0.0f ? 0.0f : Forklift.Engine.Power;
            }
            else
            {
                Forklift.Engine.PreventResistanceForOneFrame = true;
                Forklift.Engine.Power -= EngineAcceleration * brake;
                Forklift.Engine.Power *= surfaceInfo.enginePowerModifier;
                Forklift.Engine.Power = Forklift.Engine.Power < EnginePowerMin ? EnginePowerMin : Forklift.Engine.Power;
            }
        }

        bool moving = false;
        if (horizontalInput != 0.0f && horizontalInput > 0.0f)
        {
            Forklift.Wheels.PreventResistanceForOneFrame = true;
            Forklift.Wheels.Power += WheelsPower * surfaceInfo.turningSlipModifier;
            //Forklift.Wheels.Power *= Forklift.Engine.Power != 0.0f ? 1.0f : 0.0f;
            Forklift.Wheels.Power =
                Forklift.Wheels.Power > WheelsPowerMax ? WheelsPowerMax : Forklift.Wheels.Power;

            if (!rotatedLeft)
            {
                float rotAmount = -rotationAmount;
                if (rotatedRight)
                {
                    rotAmount -= rotationAmount;
                }
                leftWheel.Rotate(Vector3.up, rotAmount);
                rightWheel.Rotate(Vector3.up, rotAmount);

                rotatedRight = false;
                rotatedLeft = true;
            }

            moving = true;
        }
        else if (horizontalInput != 0.0f && horizontalInput < 0.0f)
        {
            Forklift.Wheels.PreventResistanceForOneFrame = true;
            Forklift.Wheels.Power -= WheelsPower * surfaceInfo.turningSlipModifier;
            //Forklift.Wheels.Power *= Forklift.Engine.Power != 0.0f ? 1.0f : 0.0f;
            Forklift.Wheels.Power =
                Forklift.Wheels.Power < WheelsPowerMin ? WheelsPowerMin : Forklift.Wheels.Power;

            if (!rotatedRight)
            {
                float rotAmount = rotationAmount;
                if (rotatedLeft)
                {
                    rotAmount = rotationAmount;
                }
                leftWheel.Rotate(Vector3.up, rotAmount);
                rightWheel.Rotate(Vector3.up, rotAmount);

                rotatedLeft = false;
                rotatedRight = true;
            }

            moving = true;
        }

        if (!moving)
        {
            if (rotatedLeft)
            {
                leftWheel.Rotate(Vector3.up, rotationAmount);
                rightWheel.Rotate(Vector3.up, rotationAmount);
                rotatedLeft = false;
            }
            else if (rotatedRight)
            {
                leftWheel.Rotate(Vector3.up, -rotationAmount);
                rightWheel.Rotate(Vector3.up, -rotationAmount);
                rotatedRight = false;
            }
        }
    }
}
