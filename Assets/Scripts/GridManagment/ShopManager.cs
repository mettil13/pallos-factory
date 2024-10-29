using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public enum PlaceableTypes
{
    palloGenerator,
    palloTransporter,
    palloSplitter,
    palloCollector,
    palloUpgrader,
    palloDuplicator,
    speedBooster,
    luckBooster,
    turret,
    autoCleaner
}
public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlaceableSO[] placeables;
    [SerializeField] private Placeable[] placeablesPrefs;
    [SerializeField] private GameObject cardPref;
    [SerializeField] private Transform content;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform placeableParent;

    private ShopCard[] cards;
    private float[] prices;
    private ushort[] numberOfPlaceables;

    private void Awake()
    {
        cards = new ShopCard[placeables.Length];
        prices = new float[placeables.Length];
        numberOfPlaceables = new ushort[placeables.Length];

        byte c = 0;
        while (c < placeables.Length)
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
        prices = new float[placeables.Length];
        numberOfPlaceables = new ushort[placeables.Length];

        ushort c = 0;
        while (c < placeables.Length)
        {
            prices[c] = placeables[c].startingPrice;
            numberOfPlaceables[c] = 0;
            c++;
        }

        foreach (Placeable placeable in GridManager.Instance.PlaceablesPlaced)
        {
            PlaceableTypes placeableType;
            Enum.TryParse(placeable.tag, out placeableType);
            prices[((int)placeableType)] *= placeables[((int)placeableType)].priceMultiplier;
            numberOfPlaceables[((int)placeableType)] += 1;
        }

        c = 0;
        while (c < cards.Length)
        {
            cards[c].Init(placeables[c].name, ((int)prices[c]), numberOfPlaceables[c], ((int)placeables[c].maxPurchases), null, this);
            c++;
        }
    }
    public void CloseShop()
    {

    }
    public void GenerateStructure(string structureName)
    {
        PlaceableTypes placeableType;
        Enum.TryParse(structureName, out placeableType);
        Placeable placeablePref = placeablesPrefs[((int)placeableType)];
        Vector2Int structurePosition = GridManager.Instance.FindTheNeareastFree(GridManager.Instance.GetCellFromWorldPoint(generationPoint.transform.position));
        Vector3 structurePosition3 = GridManager.Instance.GetCellCenter(structurePosition);
        Placeable placeable = GameObject.Instantiate(placeablePref.gameObject).GetComponent<Placeable>();
        placeable.transform.position = structurePosition3;
        placeable.placeableReferenced = placeables[((int)placeableType)];
        CloseShop();
    }
}
