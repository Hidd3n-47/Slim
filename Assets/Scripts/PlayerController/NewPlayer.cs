using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct Wheel
{
    public float mass;
    public Transform transform;
    public PlayerMovement.Axel axel;
}

public class NewPlayer : MonoBehaviour
{

    [SerializeField] private LayerMask groundLayerMask;

    [Header("Wheels")]
    [SerializeField] private Transform[] wheels;

    [Header("Suspension")]
    [SerializeField] private float suspensionSpringStrength    = 100.0f;
    [SerializeField] private float suspensionSpringDamper      = 15.0f;
    [SerializeField] private float maxSpringTravel             = 0.5f;
    [SerializeField] private float suspensionRestDistance      = 0.5f;
    [SerializeField] private float wheelRadius                 = 0.5f;

    [Header("Acceleration")]
    [SerializeField] private float acceleration = 25.0f;
    [SerializeField] private float maxSpeed     = 100.0f;
    [SerializeField] private float deceleration = 10.0f;
    [SerializeField] private Transform accelerationPoint;
    [SerializeField] private float dragCoefficient;

    [Header("Steering")]
    [SerializeField] private AnimationCurve turningCurve;
    [SerializeField] private float steeringStrength = 15.0f;

    private Vector3 currentCarVelocity = Vector3.zero;
    private float carVelocityRatio = 0.0f;

    private int[] wheelGrounded = new int[4];
    private bool isGrounded = false;

    private Rigidbody rigidBody;

    private float accelerationInput;
    private float steerInput;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CalculateCarVelocity();
        Movement();
    }

    void GetPlayerInput()
    {
        accelerationInput = Gamepad.current.rightTrigger.ReadValue();
        steerInput = (float)Math.Round(Gamepad.current.leftStick.value.x, 2);
    }

    private void GroundCheck()
    {
        int groundedWheels = 0;

        foreach (var g in wheelGrounded)
        {
            groundedWheels += g;
        }

        isGrounded = groundedWheels > 1;
    }

    private void CalculateCarVelocity()
    {
        currentCarVelocity = transform.InverseTransformDirection(rigidBody.linearVelocity);
        carVelocityRatio = currentCarVelocity.z / maxSpeed;
    }

    private void Suspension()
    {
        for (int i = 0; i < wheels.Length; ++i)
        {

            float maxLength = suspensionRestDistance + maxSpringTravel;

            // Try ray-cast.
            if (!Physics.Raycast(wheels[i].transform.position, Vector3.down, out var hitInfo, maxLength, groundLayerMask))
            {
                wheelGrounded[i] = 0;
                continue;
            }
            wheelGrounded[i] = 1;

            float currentSpringLength = hitInfo.distance - wheelRadius;
            float springCompression = (suspensionRestDistance - currentSpringLength) / maxSpringTravel;

            float springVelocity = Vector3.Dot(rigidBody.GetPointVelocity(wheels[i].position), Vector3.up);

            float dampForce = springVelocity * suspensionSpringDamper;

            float springForce = suspensionSpringStrength * springCompression;

            float netForce = springForce - dampForce;

            rigidBody.AddForceAtPosition(netForce * Vector3.up, wheels[i].transform.position);
        }
    }

    private void Movement()
    {
        if (isGrounded)
        {
            Acceleration();
            Deceleration();
            Turn();
            SidewaysDrag();
        }
    }

    private void Acceleration()
    {
        rigidBody.AddForceAtPosition(acceleration * accelerationInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Deceleration()
    {
        rigidBody.AddForceAtPosition(deceleration * accelerationInput * -transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Turn()
    {
        rigidBody.AddRelativeTorque(steeringStrength * steerInput * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * rigidBody.transform.up, ForceMode.Acceleration);
    }

    private void SidewaysDrag()
    {
        float currentSidewaySpeed = currentCarVelocity.x;
        float dragMagnitude = -currentSidewaySpeed * dragCoefficient;
        Vector3 dragForce = dragMagnitude * transform.right;

        rigidBody.AddForceAtPosition(dragForce, rigidBody.worldCenterOfMass, ForceMode.Acceleration);
    }

    //[SerializeField] private LayerMask groundLayerMask;

    //[Header("Wheels")]
    //[SerializeField] private List<Wheel> wheels;

    //[Header("Suspension")] 
    //[SerializeField] private float suspensionSpringStrength    = 100.0f;
    //[SerializeField] private float suspensionSpringDamper      =  15.0f;
    //[SerializeField] private float maxSuspensionSprintDistance = 0.5f;

    //[Header("Steering")]
    //[SerializeField] private AnimationCurve wheelSteeringCurve;

    //[Header("Acceleration")]
    //[SerializeField] private AnimationCurve enginePowerCurve;
    //[SerializeField] private float maxSpeed = 15.0f;

    //private float suspensionRestDistance;

    //private Rigidbody carRigidbody;

    //private void Start()
    //{
    //    carRigidbody = GetComponent<Rigidbody>();

    //    Debug.Log(wheels[0].transform.position.y);

    //    if (Physics.Raycast(wheels[0].transform.position, Vector3.down, out var hitInfo, maxSuspensionSprintDistance, groundLayerMask))
    //    {
    //        suspensionRestDistance = hitInfo.distance;
    //    }
    //}

    //// Update is called once per frame
    //private void FixedUpdate()
    //{
    //    Suspension();
    //    Steering();
    //    Acceleration();
    //}

    //private void Suspension()
    //{
    //    foreach (var wheel in wheels)
    //    {
    //        // Try ray-cast.
    //        if (!Physics.Raycast(wheel.transform.position, Vector3.down, out var hitInfo, maxSuspensionSprintDistance, groundLayerMask))
    //        {
    //            continue;
    //        }

    //        // World space direction of the spring force
    //        Vector3 springDirection = wheel.transform.up;

    //        // World space velocity of tire
    //        Vector3 wheelWorldVelocity = carRigidbody.GetPointVelocity(wheel.transform.position);

    //        // Offset from the ray cast
    //        float offset = suspensionRestDistance - hitInfo.distance;

    //        // Calculate velocity projection in spring dir
    //        float velocity = Vector3.Dot(springDirection, wheelWorldVelocity);

    //        // force
    //        // Fnet = spring - damp
    //        // Fnet = (offset * spring strength) - (vel * spring damper)
    //        float force = (offset * suspensionSpringStrength) - (velocity * suspensionSpringDamper);

    //        // apply.
    //        carRigidbody.AddForceAtPosition(springDirection * force, wheel.transform.position);
    //    }
    //}

    //private void Steering()
    //{
    //    foreach (var wheel in wheels)
    //    {
    //        if (!Physics.Raycast(wheel.transform.position, Vector3.down, out var hitInfo, maxSuspensionSprintDistance, groundLayerMask))
    //        {
    //            continue;
    //        }

    //        Vector3 steeringDirection = wheel.transform.forward;

    //        Vector3 wheelWorldVelocity = carRigidbody.GetPointVelocity(wheel.transform.position);

    //        float steeringVelocity = Vector3.Dot(steeringDirection, wheelWorldVelocity);

    //        float desiredVelocityChange = -steeringVelocity * wheelSteeringCurve.Evaluate(0.0f);

    //        float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

    //        // apply.
    //        carRigidbody.AddForceAtPosition(steeringDirection * wheel.mass * desiredAcceleration, wheel.transform.position);
    //    }
    //}

    //private void Acceleration()
    //{
    //    foreach (var wheel in wheels)
    //    {
    //        if (wheel.axel == PlayerMovement.Axel.Front || !Physics.Raycast(wheel.transform.position, Vector3.down, out var hitInfo, maxSuspensionSprintDistance, groundLayerMask))
    //        {
    //            continue;
    //        }

    //        Vector3 accelerationDirection = wheel.transform.right;

    //        float accelerationInput = Gamepad.current.rightTrigger.ReadValue();

    //        if (accelerationInput > 0.0f)
    //        {
    //            float carSpeed = Vector3.Dot(transform.right, carRigidbody.angularVelocity);

    //            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / maxSpeed);

    //            float availableTorque = enginePowerCurve.Evaluate(normalizedSpeed) * accelerationInput;
    //            Debug.Log(accelerationDirection * availableTorque);

    //            carRigidbody.AddForceAtPosition(accelerationDirection * availableTorque, transform.position);
    //        }
    //    }
    //}
}
