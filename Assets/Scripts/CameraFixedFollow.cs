using UnityEngine;

public class CameraFixedFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 previousPlayerPosition;

    private void Start()
    {
        previousPlayerPosition = player.position;
    }

    void Update()
    {
        Vector3 difference = player.position - previousPlayerPosition;

        transform.position += new Vector3(difference.x, 0.0f, difference.z);
        previousPlayerPosition = player.position;
    }
}
