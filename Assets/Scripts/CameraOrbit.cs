using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private float rotationTime   = 0.5f;
    [SerializeField] private float snappingAmount = 90.0f;

    [SerializeField] private Transform player;

    public void Activate()
    {
        transform.parent = player;
    }

    private void Update()
    {
        if(Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            StopAllCoroutines();
            StartCoroutine(RotateCamera(snappingAmount));
        }
        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            StopAllCoroutines();
            StartCoroutine(RotateCamera(-snappingAmount));
        }
    }

    IEnumerator RotateCamera(float amount)
    {
        float addedAmount  = 0f;
        float direction    = Mathf.Sign(amount);
        float targetAmount = Mathf.Abs(amount);

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
    }
}
