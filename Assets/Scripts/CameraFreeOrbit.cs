using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFreeOrbit : MonoBehaviour
{
    [SerializeField] private float maxRotationHorizontal = 180.0f;

    [SerializeField] private Transform player;

    private float previousMove = 0.0f;

    public void Activate()
    {
        transform.parent = player;
    }

    private void Update()
    {
        float move = Gamepad.current.rightStick.value.x;

        float now  = Mathf.Lerp(0.0f, maxRotationHorizontal, move);
        float prev = Mathf.Lerp(0.0f, maxRotationHorizontal, previousMove);

        transform.RotateAround(player.transform.position, Vector3.up, now - prev);

        previousMove = move;
    }
}
