using UnityEngine;
using UnityEngine.Events;

public class DeliveryZone : MonoBehaviour
{
    public UnityEvent OnDelivered;

    [SerializeField] private Transform objectiveToDeliver;
    [SerializeField] private Transform deliveryPivotPoint;

    public Transform ObjectToDeliver    => objectiveToDeliver;
    public Transform DeliveryPivotPoint => deliveryPivotPoint;
}
