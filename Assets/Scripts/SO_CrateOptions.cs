using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_CrateOptions", menuName = "Scriptable Objects/SO_CrateOptions")]
public class SO_CrateOptions : ScriptableObject
{
    [SerializeField] private List<Transform> crateList;

    public List<Transform> CrateList => crateList;
}
