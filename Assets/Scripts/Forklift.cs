using System;
using System.Collections.Generic;
using UnityEngine;

public class Forklift : MonoBehaviour
{
    public Action<Collision> OnCollideFront;
    public Action<Collision> OnCollideBack;
    public Action<Collision> OnCollideRight;
    public Action<Collision> OnCollideLeft;

    public BoxCollider ColliderFront;
    public BoxCollider ColliderBack;
    public BoxCollider ColliderRight;
    public BoxCollider ColliderLeft;

    public Force Engine = new();
    public Force Wheels = new();

    public List<Force> ExternalForces = new();


    public Transform Body;
    public float BodyRotateAmount       = 15.0f;
    public float BackwardRotationAmount = 5.0f;
    public float SidewayRotationAmount  = 10.0f;

    public bool rotatingLeft = false;
    public bool rotatingRight = false;

    public float engineMaxPower = 7.0f;
    public float engineMinPower = -3.0f;
    public float wheelsMaxPower = 300.0f;

    public Transform ForkAttachmentPoint;

    public Transform ForkliftAttachment;

    void Awake()
    {
        Engine.Type = ForceType.Translation;
        Engine.Transform = transform;

        Wheels.Type = ForceType.Rotation;
        Wheels.Transform = transform;
        Wheels.Direction = Vector3.up;
    }

    private void Update()
    {
        UpdateExternalForces();
        UpdateWheels();
        UpdateEngine();
        UpdateBodyRotation();
    }

    void OnCollisionEnter(Collision collision)
    {
        //todo refactor.
        if (collision.GetContact(0).thisCollider == ColliderFront) { OnCollideFront?.Invoke(collision);}
        if (collision.GetContact(0).thisCollider == ColliderBack) { OnCollideBack?.Invoke(collision);}
        if (collision.GetContact(0).thisCollider == ColliderRight) { OnCollideRight?.Invoke(collision);}
        if (collision.GetContact(0).thisCollider == ColliderLeft) { OnCollideLeft?.Invoke(collision); }
    }

    void UpdateWheels()
    {
        Wheels.Refresh();
    }

    void UpdateEngine()
    {

        Engine.Direction = transform.forward;
        Engine.Refresh();
    }

    void UpdateExternalForces()
    {
        ExternalForces.ForEach(f => f.Refresh());
        ExternalForces.RemoveAll(f => f.Power == 0.0f);
    }

    private void UpdateBodyRotation()
    {
        // Body rotation based off the acceleration/deceleration power. 
        float accelerationBound = Engine.Power > 0.0f ? engineMaxPower : engineMinPower;
        float rotationBound     = Engine.Power > 0.0f ? BodyRotateAmount : BackwardRotationAmount;
        float tAcceleration     = Math.Abs(Engine.Power) / Math.Abs(accelerationBound);
        float bodyAccelerationRotation = -Math.Sign(accelerationBound) * Mathf.Lerp(0.0f, rotationBound, tAcceleration);

        // Body rotation based off the turning power. 
        float turningBound = Wheels.Power > 0.0f ? wheelsMaxPower : -wheelsMaxPower;
        float tTurning = Math.Abs(Wheels.Power) / Math.Abs(turningBound);
        float bodyTurningRotation = Math.Sign(turningBound) * Mathf.Lerp(0.0f, SidewayRotationAmount, tTurning);

        var bodyRotation = Quaternion.Euler(bodyAccelerationRotation, 0.0f, bodyTurningRotation);
        Body.localRotation = bodyRotation;
    }
}
