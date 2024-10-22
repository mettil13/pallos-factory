using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureSO", menuName = "ScriptableObjects/StructureSO", order = 1)]
public class StructureSO : PlaceableSO 
{
    public float prodPerMinute;
    [Range(0, 1)] public float successProbability;
    public int capacity;
    [Range(0, 1)] public float luck;
    [Range(0, 1)] public float badLuck;
    [Range(0, 1)] public float globalBadLuck;
}