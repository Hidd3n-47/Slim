using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForkAttachment : MonoBehaviour
{
    public Transform Forks;

    public Transform Poles;

    public Transform Bottom;
    public Transform Middle;
    public Transform Top;

    public float attachmentRaiseSpeed = 2.0f;

    public Crate AttachCrate
    {
        get;
        set;
    }

    void Update()
    {
        var rightShoulder = Gamepad.current.rightShoulder;
        var leftShoulder  = Gamepad.current.leftShoulder;

        float attachMoveSpeed = 0.0f;

        if (leftShoulder.isPressed)
        {
            attachMoveSpeed += -1.0f;
        }
        if (rightShoulder.isPressed)
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

            if (Mathf.Approximately(Forks.position.y, Bottom.position.y))
            {
                AttachCrate?.Detach();
            }
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
