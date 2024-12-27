using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Pallolevel
{
    [SerializeField] public uint level;
    [SerializeField] public uint value;
    [SerializeField] public Material material;
    [SerializeField] public float scaleMultiplier;
}

[CreateAssetMenu(fileName = "pallo settings", menuName = "ScriptableObjects/palloSettingsSO", order = 1)]
public class PalloSettings : ScriptableObject
{
    [SerializeField] Pallolevel[] palloLevels;
    [SerializeField] public Material structureMaterialDefault;
    [SerializeField] public Material structureMaterialCorrupted;
    [SerializeField] public GameObject darkPallo;

    public Pallolevel GetLevel(uint level)
    {
        if (level >= palloLevels.Length)
            return palloLevels[palloLevels.Length - 1];

        return palloLevels[level];
    }

    private void OnValidate()
    {
        byte c = 0;
        while (c < palloLevels.Length)
        {
            palloLevels[c].level = ((uint)(c + 1));
            c++;
        }
    }
}