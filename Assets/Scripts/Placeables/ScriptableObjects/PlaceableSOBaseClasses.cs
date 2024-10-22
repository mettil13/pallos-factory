using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSO : ScriptableObject
{
    public string name;
    public float startingPrice;
    public float priceMultiplierForEachPurchase;
    public float maxPurchases;
}
public class BoosterSO : PlaceableSO {
    public float radius;
    [Range(0, 1)] public float globalBadLuck;
}