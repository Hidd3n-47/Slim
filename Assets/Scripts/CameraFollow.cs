using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private Transform cameraManager;

    [Range(0.0f, 1.0f)] [SerializeField] private float positionLinearInterpolationAmount = 0.2f;
    [Range(0.0f, 1.0f)] [SerializeField] private float rotationLinearInterpolationAmount = 0.2f;

    public void Activate()
    {
        cameraTarget.parent = player.transform;
        transform.parent    = cameraManager;
    }

    private void Update()
    {
        if (cameraTarget.position == transform.position)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, positionLinearInterpolationAmount);

        if (cameraTarget.rotation == transform.rotation)
        {
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, rotationLinearInterpolationAmount);
    }
}
