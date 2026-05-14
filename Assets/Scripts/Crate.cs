using JetBrains.Annotations;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private Transform frontTransform;
    [SerializeField] private Transform rearTransform;

    [SerializeField] private float rayDistance = 2.0f;

    [SerializeField] private LayerMask forkliftLayerMask;

    private bool attached = false;
    public bool Attached => attached;

    private void Update()
    {
        if (attached) return;

        Debug.DrawLine(frontTransform.position, frontTransform.position + Vector3.down * rayDistance);
        if (Physics.Raycast(frontTransform.position, Vector3.down, out var hitInfoFront, rayDistance, forkliftLayerMask))
        {
            var forklift = hitInfoFront.transform.GetComponent<Forklift>();

            transform.parent        = forklift.ForkAttachmentPoint;
            transform.localPosition = frontTransform.localPosition;
            transform.localRotation = Quaternion.identity;

            forklift.ForkliftAttachment.GetComponent<ForkAttachment>().AttachCrate = this;

            Attach();
        }

        Debug.DrawLine(rearTransform.position, rearTransform.position + Vector3.down * rayDistance);
        if (Physics.Raycast(rearTransform.position, Vector3.down, out var hitInfoRear, rayDistance, forkliftLayerMask))
        {
            var forklift = hitInfoRear.transform.GetComponent<Forklift>();

            transform.parent        = forklift.ForkAttachmentPoint;
            transform.localPosition = -rearTransform.localPosition;
            transform.localRotation = Quaternion.identity;

            forklift.ForkliftAttachment.GetComponent<ForkAttachment>().AttachCrate = this;

            Attach();
        }
    }

    private void Attach()
    {
        attached = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Detach()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        attached = false;
    }
}
