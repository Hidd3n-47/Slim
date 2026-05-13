using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFader : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Material obstructionMaterial;

    private readonly Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private readonly List<Renderer> currentObstructions = new List<Renderer>();

    private bool ready = false;

    IEnumerator Start()
    {
        yield return null;
        ready = true;
    }

    void Update()
    {
        if (!ready) return;
        CheckObstructions();
    }

    void CheckObstructions()
    {
        for (int i = 0; i < currentObstructions.Count; i++)
        {
            Renderer r = currentObstructions[i];
            if (r != null && originalMaterials.ContainsKey(r))
            {
                r.sharedMaterials = originalMaterials[r];
            }
        }

        currentObstructions.Clear();

        Vector3 dir = player.position - transform.position;
        float dist = Vector3.Distance(player.position, transform.position);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, dist);

        foreach (var hit in hits)
        {
            Renderer r = hit.collider.GetComponentInChildren<Renderer>();
            if (r == null)
                r = hit.collider.GetComponentInParent<Renderer>();

            if (r == null)
                continue;

            if (!originalMaterials.ContainsKey(r))
            {
                originalMaterials[r] = r.sharedMaterials;
            }

            Material[] mats = new Material[r.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = obstructionMaterial;

            r.materials = mats;

            currentObstructions.Add(r);
        }
    }
}
