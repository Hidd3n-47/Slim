using UnityEngine;

public class RandomWoodMaterial : MonoBehaviour
{
    public Material mat;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // "_Hue"
        // "_Saturation"
        // "_Brightness"

        var m = GetComponentInChildren<MeshRenderer>().material;
        var p = m.GetPropertyNames(MaterialPropertyType.Float);
        m.SetFloat("_Hue", Random.Range(-15, 15));
        m.SetFloat("_Saturation", Random.Range(0.6f, 1.2f));
        m.SetFloat("_Brightness", Random.Range(-0.05f, 0.05f));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
