using System.Linq;
using UnityEngine;

public class WallColor : MonoBehaviour
{
    [SerializeField] private Color color;

    private Material wallMaterial;

    private void Start()
    {
        wallMaterial = GetComponent<Renderer>().materials.FirstOrDefault(x => x.name == "WallBottom_M (Instance)");

        if (wallMaterial)
        {
            wallMaterial.SetColor("_Color", color);
        }
    }
}
