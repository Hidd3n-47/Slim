using UnityEngine;

public class RandomWoodMaterial : MonoBehaviour
{
    public Material mat;
    public float hueMin = -1.0f;
    public float hueMax = 1.0f;
    public float saturationMin = -1.0f;
    public float saturationMax = 1.0f;
    public float brightnessMin = -1.0f;
    public float brightnessMax = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // "_Hue"
        // "_Saturation"
        // "_Brightness"

        var m = GetComponentInChildren<MeshRenderer>().material;
        var p = m.GetPropertyNames(MaterialPropertyType.Float);
        m.SetFloat("_Hue", Random.Range(hueMin, hueMax));
        m.SetFloat("_Saturation", Random.Range(saturationMin, saturationMax));
        m.SetFloat("_Brightness", Random.Range(brightnessMin, brightnessMax));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
