using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PalloBotSO", menuName = "ScriptableObjects/PalloBotSO", order = 1)]
public class PalloBotSO : PlaceableSO
{
    public float prodPerMinute;
    [Range(0, 1)] public float successProbability;
    public float radius;
    [Range(0, 1)] public float globalBadLuck;
}