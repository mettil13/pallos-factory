using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSO : ScriptableObject
{
    public string name;
    public float price;
}



public class BoosterSO : PlaceableSO {
    public float radius;
    [Range(0, 1)] public float globalBadLuck;
}

[CreateAssetMenu(fileName = "LuckBoosterSO", menuName = "ScriptableObjects/LuckBoosterSO", order = 1)]
public class LuckBoosterSO : BoosterSO {
    public float additionalLuck;
}

[CreateAssetMenu(fileName = "SpeedBoosterSO", menuName = "ScriptableObjects/SpeedBoosterSO", order = 1)]
public class SpeedBoosterSO : BoosterSO {
    public float additionalSpeed;
}

[CreateAssetMenu(fileName = "PalloBotSO", menuName = "ScriptableObjects/PalloBotSO", order = 1)]
public class PalloBotSO : PlaceableSO {
    public float prodPerMinute;
    [Range(0, 1)] public float successProbability;
    public float radius;
    [Range(0, 1)] public float globalBadLuck;
}