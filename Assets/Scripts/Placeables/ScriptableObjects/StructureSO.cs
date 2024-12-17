using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureSO", menuName = "ScriptableObjects/StructureSO", order = 1)]
public class StructureSO : PlaceableSO 
{
    public float prodPerSecond;
    [Range(0, 1)] public float successProbability;
    public int capacity;
    [Range(0, 1)] public float luck;
    [Range(0, 1)] public float badLuck;


    public override void SetPlaceableInfo(Placeable placeable)
    {
        if (placeable.IsStructure())
        {
            Structure structure = (Structure)placeable;
            structure.capacity = ((short)capacity);
            structure.processingTime = 1 / prodPerSecond;
            structure.boostedPalloGenerationProbability = luck;
            structure.darkPalloGenerationProbability = badLuck;
        }
        base.SetPlaceableInfo(placeable);
    }
}