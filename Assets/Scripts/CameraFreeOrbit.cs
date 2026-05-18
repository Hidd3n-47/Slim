using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFreeOrbit : MonoBehaviour
{
    [SerializeField] private float maxRotationHorizontal = 180.0f;

    [SerializeField] private Transform player;

    [SerializeField] private float lerpSpeed = 0.1f;

    private float previousMove = 0.0f;

    public void Activate()
    {
        transform.parent = player;
    }

    private void Update()
    {
        float move = Gamepad.current.rightStick.value.x;

        move = Mathf.Lerp(previousMove, move, lerpSpeed);

        float now  = Mathf.Lerp(0.0f, maxRotationHorizontal, Mathf.Abs(move)) * Mathf.Sign(move);
        float prev = Mathf.Lerp(0.0f, maxRotationHorizontal, Mathf.Abs(previousMove)) * Mathf.Sign(previousMove);

        transform.RotateAround(player.transform.position, Vector3.up, prev - now);

        previousMove = move;
    }
}
