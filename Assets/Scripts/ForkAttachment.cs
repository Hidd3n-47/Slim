using System;
using UnityEngine;

public class ForkAttachment : MonoBehaviour
{
    public Transform Forks;

    public Transform Poles;

    public Transform Bottom;
    public Transform Middle;
    public Transform Top;

    public float attachmentRaiseSpeed = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float attachMoveSpeed = 0.0f;

        if (Input.GetKey(KeyCode.Q))
        {
            attachMoveSpeed += -1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            attachMoveSpeed += 1.0f;
        }

        //todo since we always move the forks, could abstract that out.
        if (Forks.position.y <= Middle.position.y)
        {
            // move forks only
            float delta = attachMoveSpeed * attachmentRaiseSpeed * Time.deltaTime;

            Forks.position += new Vector3(0.0f, delta, 0.0f);
            var pos = Forks.position;
            pos.y = Math.Clamp(pos.y, Bottom.position.y, Middle.position.y + 0.1f);
            Forks.position = pos;
        }
        else
        {
            // move Poles and forks.
            float delta = attachMoveSpeed * attachmentRaiseSpeed * Time.deltaTime;
            var deltaVec = new Vector3(0.0f, delta, 0.0f);
            Forks.position += deltaVec;

            var posForks = Forks.position;
            posForks.y = Math.Clamp(posForks.y, Middle.position.y, Top.position.y);
            Forks.position = posForks;
            Poles.position += deltaVec;

            var pos = Poles.position;
            pos.y = Math.Clamp(pos.y, Middle.position.y, Top.position.y);
            Poles.position = pos;

        }
    }
}
