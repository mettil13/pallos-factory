using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour, IPointerClickHandler {

    public TextMeshProUGUI productNameText, priceText, quantityText;
    public Image thumbnail;

    private string productName;
    private int price;
    private int currentQuantity;
    private int maxQuantity;
    ShopManager shop;

    public void Init(string productName, int price, int currentQuantity, int maxQuantity, Sprite thumbnail, ShopManager shop) 
    {
        this.productName = productName;
        productNameText.text = productName;
        this.price = price;
        priceText.text = price + " <sprite name=\"pallo\">";
        this.currentQuantity = currentQuantity;
        this.maxQuantity = maxQuantity;
        quantityText.text = currentQuantity + "/" + maxQuantity;
        this.thumbnail.sprite = thumbnail;

        this.shop = shop;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        shop.GenerateStructure(productName);
        Debug.Log("card pressed");
    }
}
