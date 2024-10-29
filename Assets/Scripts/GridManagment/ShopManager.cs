using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlaceableSO[] placeables;
    [SerializeField] private GameObject cardPref;
    [SerializeField] private Transform content;

    private ShopCard[] cards;


    private void Awake()
    {
        cards = new ShopCard[placeables.Length];
        byte c = 0;
        while (c < placeables.Length)
        {
            cards[c] = GameObject.Instantiate(cardPref).GetComponent<ShopCard>();
            //cards[c].Init(placeables[c].name, placeables[c].startingPrice, )
            c++;
        }
    }
    public void OpenShop()
    {
        
    }
    public void CloseShop()
    {

    }
    public void GenerateStructure()
    {
        CloseShop();
    }
}
