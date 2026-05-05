using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject model;
        public WheelCollider collider;
        public Axel axel;
    }

    [SerializeField] private float rotationRate = 20.0f;
    [SerializeField] private float speed        = 20.0f;
    [SerializeField] private float reverseSpeed = 15.0f;
    [SerializeField] private float attachmentMoveSpeed = 15.0f;
    [SerializeField] private float maxLiftSpeed        = 15.0f;

    [SerializeField] private Transform movingThing;
    [SerializeField] private Transform movingThingPivot;

    public float maxAccel = 30.0f;
    public float brakeAccel = 50.0f;

    public float turnSens = 1.0f;
    public float maxSteerAngle = 80.0f;

    public float moveInput;
    public float steerInput;
    public Vector3 centerOfMass;

    private Rigidbody rb;

    public List<Wheel> wheels = new List<Wheel>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }
    private void LateUpdate()
    {
        Move();
        Steer();
    }

    void GetInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.motorTorque = moveInput * maxAccel * 2000.0f * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {

            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSens * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        GetInput();

        float move = 0.0f;
        float turn = 0.0f;
        float attachMoveSpeed = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            move += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += -1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            turn += -1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            turn += 1.0f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            attachMoveSpeed += -1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            attachMoveSpeed += 1.0f;
        }

        var rb = movingThing.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * attachMoveSpeed * attachmentMoveSpeed);

        Vector3 vel = rb.linearVelocity;
        vel.y = Mathf.Clamp(vel.y, -maxLiftSpeed, maxLiftSpeed);
        rb.linearVelocity = vel;

        if (attachMoveSpeed == 0.0f || movingThing.localPosition.y <= 0.045f || movingThing.localPosition.y >= 1.9f)
        {
            rb.linearVelocity = Vector3.zero;
        }


        // If we are going backwards, flip the direction
        if (move < 0.0f)
        {
            turn *= -1;
        }

        Transform t = GetComponent<Transform>();

        //t.eulerAngles += new Vector3(0.0f, turn * rotationRate * Time.deltaTime, 0.0f) * (!move.Equals(0.0f) ? 1.0f : 0.0f);
        var delta = t.forward * move * (move < 0.0f ? reverseSpeed : speed);

        //t.position += delta * Time.deltaTime;
    }
}
