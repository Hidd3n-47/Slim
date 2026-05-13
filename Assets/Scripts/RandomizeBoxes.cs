using System.Collections.Generic;
using UnityEngine;

public class RandomizeBoxes : MonoBehaviour
{
    [SerializeField] private SO_CrateOptions crates;
    [SerializeField] private List<Transform> pivotPoints;

    private void Awake()
    {
        foreach (var point in pivotPoints)
        {
            Instantiate(crates.CrateList[Random.Range(0, crates.CrateList.Count)], point);
        }
    }
}
