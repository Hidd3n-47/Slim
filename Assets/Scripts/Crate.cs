using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private Transform frontTransform;
    [SerializeField] private Transform rearTransform;

    public Transform RearTransform => rearTransform;
}
