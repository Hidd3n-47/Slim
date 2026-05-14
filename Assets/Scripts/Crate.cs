using JetBrains.Annotations;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public void Attach()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public bool Detach()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;

        return true;
    }
}
