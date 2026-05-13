using UnityEngine;

[CreateAssetMenu(fileName = "SO_RandomWoodMaterial", menuName = "Scriptable Objects/SO_RandomCrateColors")]
public class SO_RandomWoodMaterial : ScriptableObject
{
    [SerializeField] private float hueMin;
    [SerializeField] private float hueMax;
    [SerializeField] private float saturationMin;
    [SerializeField] private float saturationMax;
    [SerializeField] private float brightnessMin;
    [SerializeField] private float brightnessMax;

    public float HueMin        => hueMin;
    public float HueMax        => hueMax;
    public float SaturationMin => saturationMin;
    public float SaturationMax => saturationMax;
    public float BrightnessMin => brightnessMin;
    public float BrightnessMax => brightnessMax;
}
