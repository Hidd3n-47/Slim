using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float rotationRate = 20.0f;
    [SerializeField] private float speed        = 20.0f;
    [SerializeField] private float reverseSpeed = 15.0f;
    [SerializeField] private float attachmentMoveSpeed = 15.0f;
    [SerializeField] private float maxLiftSpeed        = 15.0f;

    [SerializeField] private Transform movingThing;
    [SerializeField] private Transform movingThingPivot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        t.eulerAngles += new Vector3(0.0f, turn * rotationRate * Time.deltaTime, 0.0f) * (!move.Equals(0.0f) ? 1.0f : 0.0f);
        var delta = t.forward * move * (move < 0.0f ? reverseSpeed : speed);

        t.position += delta * Time.deltaTime;
    }
}
