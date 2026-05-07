using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Force Engine = new Force();
    public Force Wheels = new Force();

    public List<Force> ExternalForces = new List<Force>();

    public bool IsDriving = false;

    public Transform Body;
    public float BodyRotateAmount = 15.0f;

    public bool rotatingLeft = false;
    public bool rotatingRight = false;

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
        // If we are swaying left and already rotated to the left, reset rotation.
        if (Wheels.Power < 0.0f && !rotatingLeft)
        {
            if (rotatingRight)
            {
                Body.Rotate(Vector3.forward, -BodyRotateAmount, Space.Self);
            }
            // feel deactivate varient by id on the right?
            Body.Rotate(Vector3.forward, -BodyRotateAmount, Space.Self);
            rotatingLeft = true;
            rotatingRight = false;
        }

        // If we are swaying right and already rotated to the right, reset rotation.
        if (Wheels.Power > 0.0f && !rotatingRight)
        {
            if (rotatingLeft)
            {
                Body.Rotate(Vector3.forward, BodyRotateAmount, Space.Self);
            }
            // feel deactivate varient by id on the left?
            Body.Rotate(Vector3.forward, BodyRotateAmount, Space.Self);
            rotatingLeft = false;
            rotatingRight = true;
        }

        if (Wheels.Power == 0.0f)
        {
            if (rotatingLeft)
            {
                Body.Rotate(Vector3.forward, BodyRotateAmount, Space.Self);
            }
            else if (rotatingRight)
            {
                Body.Rotate(Vector3.forward, -BodyRotateAmount, Space.Self);
            }

            rotatingLeft = false;
            rotatingRight = false;
        }

        // If wheel power > 0 rotate to the right by 15 degrees swaying the car.
        // If wheel power < 0 rotate to the left by 15 (so -15) degrees

        Wheels.Refresh();
    }

    void UpdateEngine()
    {
        if ((Engine.Power > 0 || Engine.Power < 0) && !IsDriving)
        {
            IsDriving = true;
            // Try activate some feel like tilting the car up due to accelerating.
            Body.Rotate(Vector3.right, BodyRotateAmount, Space.Self);
        }

        if (Engine.Power == 0 && IsDriving)
        {
            IsDriving = false;
            // reset the feel to be stopped due to being done driving.
            Body.Rotate(Vector3.right, -BodyRotateAmount, Space.Self);
        }

        Engine.Direction = transform.forward;
        Engine.Refresh();
    }

    void UpdateExternalForces()
    {
        List<Force> toRemove = new List<Force>();

        foreach (Force f in ExternalForces)
        {
            f.Refresh();
            if (f.Power == 0.0f)
            {
                toRemove.Add(f);
            }
        }

        foreach (var f in toRemove)
        {
            ExternalForces.Remove(f);
        }
    }
}
