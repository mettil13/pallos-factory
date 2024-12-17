using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSO : ScriptableObject
{
    public string name;
    public float startingPrice;
    public float priceMultiplier;
    [Range(0, 1)] public float globalBadLuck;

    public virtual void SetPlaceableInfo(Placeable placeable) 
    {
        PlayerManager.instance.globalLuck += globalBadLuck;
    }
}
public class BoosterSO : PlaceableSO 
{
    public float radius;
}