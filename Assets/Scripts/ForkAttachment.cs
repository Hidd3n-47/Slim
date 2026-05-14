using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ForkAttachment : MonoBehaviour
{
    [FormerlySerializedAs("Forks")] [SerializeField] private Transform forks;
    [FormerlySerializedAs("Poles")] [SerializeField] private Transform poles;

    [FormerlySerializedAs("Bottom")] [SerializeField] private Transform bottom;
    [FormerlySerializedAs("Middle")] [SerializeField] private Transform middle;
    [FormerlySerializedAs("Top")   ] [SerializeField] private Transform top;

    [SerializeField] private Transform crateAttachmentPoint;

    [SerializeField] private float rayDistance = 0.03f;
    [SerializeField] private LayerMask crateLayerMask;

    [SerializeField] private float attachmentRaiseSpeed = 2.0f;

    private Crate attachedCrate;

    private void Update()
    {
        TryAttach();

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

        if (forks.position.y <= middle.position.y)
        {
            // Move forks only.
            float delta = attachMoveSpeed * attachmentRaiseSpeed * Time.deltaTime;

            forks.position += new Vector3(0.0f, delta, 0.0f);
            var pos = forks.position;
            pos.y = Math.Clamp(pos.y, bottom.position.y, middle.position.y + 0.1f);
            forks.position = pos;

            if (Mathf.Approximately(forks.position.y, bottom.position.y))
            {
                if (attachedCrate?.Detach() == true)
                {
                    attachedCrate = null;
                }
            }
        }
        else
        {
            // Move Poles and Forks.
            float delta = attachMoveSpeed * attachmentRaiseSpeed * Time.deltaTime;
            var deltaVec = new Vector3(0.0f, delta, 0.0f);
            forks.position += deltaVec;

            var posForks = forks.position;
            posForks.y = Math.Clamp(posForks.y, middle.position.y, top.position.y);
            forks.position = posForks;
            poles.position += deltaVec;

            var pos = poles.position;
            pos.y = Math.Clamp(pos.y, middle.position.y, top.position.y);
            poles.position = pos;

        }
    }

    private void TryAttach()
    {
        // Already attached, no need to try and attach again.
        if (attachedCrate) return;

        Debug.DrawLine(crateAttachmentPoint.position, crateAttachmentPoint.position + Vector3.up * rayDistance);
        if (Physics.Raycast(crateAttachmentPoint.position, Vector3.down, out var hitInfo, rayDistance, crateLayerMask))
        {
            attachedCrate = hitInfo.transform.GetComponent<Crate>();
            attachedCrate.Attach();

            hitInfo.transform.parent = crateAttachmentPoint;

            hitInfo.transform.localPosition = Vector3.zero;
            hitInfo.transform.localRotation = Quaternion.identity;
        }
    }
}
