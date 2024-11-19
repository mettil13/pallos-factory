using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[System.Serializable] public enum PlaceableTypes
{
    palloGenerator,
    palloTransporter,
    palloCollector,
    palloSplitter,
    palloUpgrader,
    palloDuplicator,
    speedBooster,
    luckBooster,
    turret,
    autoCleaner
}
[System.Serializable] public struct PlaceableStruct
{
    [SerializeField] public PlaceableSO placeableSO;
    [SerializeField] public Placeable pref;
    [SerializeField] public Sprite tumbnail;
}


public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlaceableStruct[] placeableStructs; 
    [SerializeField] private GameObject cardPref;
    [SerializeField] private Transform content;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform placeableParent;
    [SerializeField] private UnityEvent OnCloseEvent;

    [Header("cards animations")]
    [SerializeField] public AnimationCurve selectCardCurve;

    private ShopCard[] cards;
    private float[] prices;
    private ushort[] numberOfPlaceables;

    private void Awake()
    {
        cards = new ShopCard[placeableStructs.Length];
        prices = new float[placeableStructs.Length];
        numberOfPlaceables = new ushort[placeableStructs.Length];

        byte c = 0;
        while (c < placeableStructs.Length)
        {
            cards[c] = GameObject.Instantiate(cardPref).GetComponent<ShopCard>();
            cards[c].transform.SetParent(content);
            //cards[c].Init(placeables[c].name, placeables[c].startingPrice, )
            c++;
        }

        //Invoke(nameof(OpenShop), 1f);
    }
    public void OpenShop()
    {
        prices = new float[placeableStructs.Length];
        numberOfPlaceables = new ushort[placeableStructs.Length];

        ushort c = 0;
        while (c < placeableStructs.Length)
        {
            prices[c] = placeableStructs[c].placeableSO.startingPrice;
            numberOfPlaceables[c] = 0;
            c++;
        }

        foreach (Placeable placeable in GridManager.Instance.PlaceablesPlaced)
        {
            PlaceableTypes placeableType;
            Enum.TryParse(placeable.tag, out placeableType);
            prices[((int)placeableType)] *= placeableStructs[((int)placeableType)].placeableSO.priceMultiplier;
            numberOfPlaceables[((int)placeableType)] += 1;
        }

        c = 0;
        while (c < cards.Length)
        {
            cards[c].Init(placeableStructs[c].placeableSO.name, ((int)prices[c]), numberOfPlaceables[c], ((int)placeableStructs[c].placeableSO.maxPurchases), placeableStructs[c].tumbnail, this);
            c++;
        }
    }
    public void CloseShop()
    {
        OnCloseEvent.Invoke();
    }
    public void GenerateStructure(string structureName)
    {
        PlaceableTypes placeableType;
        Enum.TryParse(structureName, out placeableType);
        Placeable placeablePref = placeableStructs[((int)placeableType)].pref;
        Vector2Int structurePosition = GridManager.Instance.FindTheNeareastFree(GridManager.Instance.GetCellFromWorldPoint(generationPoint.transform.position));
        Vector3 structurePosition3 = GridManager.Instance.GetCellCenter(structurePosition);
        Placeable placeable = GameObject.Instantiate(placeablePref.gameObject).GetComponent<Placeable>();
        placeable.transform.position = structurePosition3;
        placeable.placeableReferenced = placeableStructs[((int)placeableType)].placeableSO;
        placeable.Select();
        CloseShop();
    }

    public void CannotBuyError(uint productPrice)
    {
        Debug.LogError("cannot buy item ( " + productPrice + " ) you have only : " + PlayerManager.instance.CurrentPoints);
    }
    public void ReachedMaximumError(uint currentQuantity, uint maxQuantity)
    {
        Debug.LogError("cannot buy item ( quantity : " + currentQuantity + " ) maximum : " + maxQuantity);
    }
}
