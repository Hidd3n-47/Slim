using UnityEngine;

public class RandomWoodMaterial : MonoBehaviour
{
    [SerializeField] private SO_RandomWoodMaterial woodMaterialParameters;

    private void Start()
    {
        var material = GetComponentInChildren<MeshRenderer>().material;

        material.SetFloat("_Hue", Random.Range(woodMaterialParameters.HueMin, woodMaterialParameters.HueMax));
        material.SetFloat("_Saturation", Random.Range(woodMaterialParameters.SaturationMin, woodMaterialParameters.SaturationMax));
        material.SetFloat("_Brightness", Random.Range(woodMaterialParameters.BrightnessMin, woodMaterialParameters.BrightnessMax));
    }
}
