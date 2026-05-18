using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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

    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform leftWheel;
    public Transform rightWheel;

    public float rotationAmount = 45.0f;

    public float enginePowerPercentageToTriggerTurningLock = 0.6f;
    public float wheelLerpAmountMaxLock = 0.3f;
    public float wheelLerpAmountMinLock = 0.6f;
    public float wheelMaxTurnPower = 1.5f;

    public float previousMaxPower = 0.0f;

    public float bounceBackPercentage = 0.4f;
    public float rotationCorrectionOnCollision = 100.0f;

    public float WheelTurningRate = 25.0f;
    public float WheelTurningRateFront = 15.0f;

    public float minFov = 60;
    public float maxFov = 120.0f;

    public float bootTimer = 1.5f;
    public float timer;


    private void Awake()
    {
        timer = bootTimer;
        //surfaceLayerMask = LayerMask.NameToLayer("GroundSurface");

        // camera parenting to current transform.
        //todo this is a problem as you cannot runtime change these values.
        Forklift.Engine.Resistance = EngineResistance;
        Forklift.Wheels.Resistance = WheelsResistance;

        Forklift.OnCollideFront += (Collision collision) =>
        {
            if (collision.gameObject.CompareTag("Cone"))
            {
                float t = Mathf.Lerp(10.0f, 20.0f, Math.Abs(Forklift.Engine.Power) / Forklift.engineMaxPower);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(Random.Range(-20.0f, -45.0f), Forklift.transform.right) * Forklift.transform.forward * t, ForceMode.Impulse);
                return;
            }

            Forklift.Engine.Power *= -bounceBackPercentage;
            Forklift.ExternalForces.Add(new Force()
            {
                Type       = ForceType.Rotation,
                Transform  = Forklift.transform,
                Power      = rotationCorrectionOnCollision,
                Resistance = Forklift.Engine.Resistance,
                Direction  = Forklift.transform.up
            });
            // add feel or particles for the bump.
        };
        Forklift.OnCollideBack += (Collision collision) =>
        {
            if (collision.gameObject.CompareTag("Cone"))
            {
                float t = Mathf.Lerp(10.0f, 20.0f, Math.Abs(Forklift.Engine.Power) / Forklift.engineMaxPower);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(Random.Range(-20.0f, -45.0f), Forklift.transform.right) * -Forklift.transform.forward * t, ForceMode.Impulse);
                return;
            }

            Forklift.Engine.Power *= -bounceBackPercentage;
            Forklift.ExternalForces.Add(new Force()
            {
                Type       = ForceType.Rotation,
                Transform  = Forklift.transform,
                Power      = rotationCorrectionOnCollision,
                Resistance = Forklift.Engine.Resistance,
                Direction  = Forklift.transform.up
            });
            // add feel or particles for the bump.
        };
        Forklift.OnCollideLeft += (Collision collision) =>
        {
            if (collision.gameObject.CompareTag("Cone"))
            {
                float t = Mathf.Lerp(10.0f, 20.0f, Math.Abs(Forklift.Engine.Power) / Forklift.engineMaxPower);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(Random.Range(-20.0f, -45.0f), Forklift.transform.forward) * Forklift.transform.right * t, ForceMode.Impulse);
                return;
            }

            Forklift.Wheels.Power *= -bounceBackPercentage;
            // add feel or particles for the bump.
        };
        Forklift.OnCollideRight += (Collision collision) =>
        {
            if (collision.gameObject.CompareTag("Cone"))
            {
                float t = Mathf.Lerp(10.0f, 20.0f, Math.Abs(Forklift.Engine.Power) / Forklift.engineMaxPower);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(Random.Range(-20.0f, -45.0f), Forklift.transform.right) * Forklift.transform.forward * t, ForceMode.Impulse);
                return;
            }

            Forklift.Wheels.Power *= -bounceBackPercentage;
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
        Forklift.Engine.Resistance = EngineResistance;
        Forklift.Wheels.Resistance = WheelsResistance;

        Forklift.engineMaxPower = EnginePowerMax;
        Forklift.engineMinPower = EnginePowerMin;
        Forklift.wheelsMaxPower = WheelsPowerMax;

        UnityEngine.Debug.Log(timer);
        if(Gamepad.current.circleButton.isPressed && timer > 0.0f)
        {
            timer -= Time.deltaTime;
            maxFov = 90.0f;
            Forklift.engineMaxPower = 20.0f;
            EnginePowerMax = 20.0f;
        }
        else
        {
            Forklift.engineMaxPower = 10.0f;
            EnginePowerMax = 10.0f;
            maxFov = 70.0f;
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0.0f, bootTimer);
        }

        Camera.fieldOfView = Mathf.Lerp(minFov, maxFov, Math.Abs(Forklift.Engine.Power) / Forklift.engineMaxPower);
    }

    private void FixedUpdate()
    {
        DetectSurface();

        float acceleration = Gamepad.current.rightTrigger.ReadValue();
        float brake        = Gamepad.current.leftTrigger.ReadValue();
        float maxPower = Mathf.Lerp(0.0f, EnginePowerMax, acceleration);
        if (previousMaxPower == 0.0f || previousMaxPower < maxPower)
        {
            previousMaxPower = maxPower;
        }
        else
        {
            previousMaxPower -= EngineResistance * Time.deltaTime;
        }

        float horizontalInput = (float)Math.Round(Gamepad.current.leftStick.value.x, 2);

        if (acceleration != 0.0f)
        {
            // todo investigate what I was thinking with this case: if we are revering and accelerate?
            if (Forklift.Engine.Power < 0)
            {
                Forklift.Engine.Power += Brake;
                //todo this max should be inside the forklift get and set property.
                Forklift.Engine.Power = Math.Min(Forklift.Engine.Power, 0);
            }
            else
            {
                Forklift.Engine.PreventResistanceForOneFrame = true;
                Forklift.Engine.Power += EngineAcceleration;
                //todo this max should be inside the forklift get and set property.
                Forklift.Engine.Power = Math.Min(Forklift.Engine.Power, previousMaxPower);
            }
        }

        if (brake != 0.0f)
        {
            if (Forklift.Engine.Power > 0)
            {
                Forklift.Engine.PreventResistanceForOneFrame = true;
                Forklift.Engine.Power -= EngineAcceleration * brake;
                //todo this max should be inside the forklift get and set property.
                Forklift.Engine.Power = Math.Max(Forklift.Engine.Power, 0);
            }
            else
            {
                Forklift.Engine.PreventResistanceForOneFrame = true;
                Forklift.Engine.Power -= EngineAcceleration * brake;
                //todo this max should be inside the forklift get and set property.
                Forklift.Engine.Power = Forklift.Engine.Power < EnginePowerMin ? EnginePowerMin : Forklift.Engine.Power;
            }
        }

        // Player is breaking and accelerating so allow resistance to work.
        Forklift.Engine.PreventResistanceForOneFrame = !Mathf.Approximately(acceleration, brake) && Forklift.Engine.PreventResistanceForOneFrame;
        // Apply the surface modifier to the engine power.
        Forklift.Engine.Power *= surfaceInfo.enginePowerModifier;

        // Apply lock up on the turning axis.
        //float lockupPower = enginePowerPercentageToTriggerTurningLock * EnginePowerMax;
        //float turning = WheelsPower;
        //if (acceleration > 0.0f &&
        //    acceleration > lockupPower)
            float lockupPower = enginePowerPercentageToTriggerTurningLock;
        float turning = WheelsPower;
        if (acceleration > 0.0f && acceleration > lockupPower)
        {
            turning = Mathf.Lerp(WheelsPower, wheelMaxTurnPower,
                (acceleration - lockupPower) / (1.0f - lockupPower));
        }

        if (horizontalInput != 0.0f && horizontalInput > 0.0f)
        {
            Forklift.Wheels.PreventResistanceForOneFrame = true;
            Forklift.Wheels.Power += turning * surfaceInfo.turningSlipModifier;
            //todo this max should be inside the forklift get and set property.
            Forklift.Wheels.Power = Math.Min(Forklift.Wheels.Power, WheelsPowerMax);
        }
        else if (horizontalInput != 0.0f && horizontalInput < 0.0f)
        {
            Forklift.Wheels.PreventResistanceForOneFrame = true;
            Forklift.Wheels.Power -= turning * surfaceInfo.turningSlipModifier;
            //todo this max should be inside the forklift get and set property.
            Forklift.Wheels.Power = Math.Max(Forklift.Wheels.Power, WheelsPowerMin);
        }

        // Wheels rotation based off the turning input. 
        float t = Math.Abs(horizontalInput);
        float rotation = Math.Sign(horizontalInput) * Mathf.Lerp(0.0f, rotationAmount, t);

        WheelRotation(rotation);

    }

    private float wheelSpinX = 0f;

    void WheelRotation(float lockedRotation)
    {
        float powerPercentage = Forklift.Engine.Power / (Forklift.Engine.Power > 0.0f ? EnginePowerMax : EnginePowerMin);
        float rotationAmount = Mathf.Lerp(0.0f, WheelTurningRate, powerPercentage) * Math.Sign(Forklift.Engine.Power);

        wheelSpinX += rotationAmount;

        leftWheel.localRotation  = Quaternion.Euler(wheelSpinX, -lockedRotation, 0f);
        rightWheel.localRotation = Quaternion.Euler(wheelSpinX, -lockedRotation, 0f);

        frontLeftWheel.localRotation = Quaternion.Euler(wheelSpinX, 0.0f, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(wheelSpinX, 0.0f, 0f);
    }
}
