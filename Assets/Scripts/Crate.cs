using UnityEngine;

public class Crate : MonoBehaviour
{
    private ForkAttachment parentAttachment;

    private bool delivered = false;

    public void Attach(ForkAttachment attachment)
    {
        parentAttachment = attachment;

        GetComponent<Rigidbody>().isKinematic = true;
    }

    public bool Detach()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var zone = other.GetComponentInParent<DeliveryZone>();
        if (!zone || zone.ObjectToDeliver != transform || delivered)
        {
            return;
        }

        transform.parent = zone.DeliveryPivotPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        parentAttachment.RemoveCrate();

        zone.OnDelivered?.Invoke();

        Destroy(zone);
        delivered = true;
    }
}
