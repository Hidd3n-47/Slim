using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private float rotationTime   = 0.5f;
    [SerializeField] private float snappingAmount = 90.0f;

    [SerializeField] private Transform player;

    private float addedAmount;
    private float targetAmount;
    private float direction;

    public void Activate()
    {
        transform.parent = player;
    }

    private void Update()
    {
        if(Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            StopCurrentAnimation();
            StartCoroutine(RotateCamera(snappingAmount));
        }
        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            StopCurrentAnimation();
            StartCoroutine(RotateCamera(-snappingAmount));
        }
    }

    private void StopCurrentAnimation()
    {
        transform.RotateAround(player.transform.position, Vector3.up, (targetAmount - addedAmount) * direction);

        targetAmount = 0.0f;
        addedAmount  = 0.0f;

        StopAllCoroutines();
    }

    private IEnumerator RotateCamera(float amount)
    {
        direction    = Mathf.Sign(amount);
        targetAmount = Mathf.Abs(amount);

        while (addedAmount < targetAmount)
        {
            float step = targetAmount / rotationTime * Time.deltaTime;

            if (addedAmount + step > targetAmount)
            {
                step = targetAmount - addedAmount;
            }

            transform.RotateAround(player.transform.position, Vector3.up, step * direction);

            addedAmount += step;

            yield return null;
        }

        addedAmount  = 0.0f;
        targetAmount = 0f;
    }
}
