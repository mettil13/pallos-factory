using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour, IPointerClickHandler {

    [SerializeField] public TextMeshProUGUI productNameText, priceText, quantityText;
    [SerializeField] public Image thumbnail;
    [SerializeField] public Image backGround;

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

        if (PlayerManager.instance.CurrentPoints < price) { priceText.color = Color.red; UnActive(); }
        else { priceText.color = Color.green; Active(); }
        if (this.currentQuantity >= this.maxQuantity) UnActive();
    }

    public void UnActive()
    {
        backGround.color = Color.gray;
        productNameText.color = Color.gray;
        thumbnail.color = Color.gray;
        quantityText.color = Color.gray;
    }
    public void Active()
    {
        backGround.color = Color.white;
        productNameText.color = Color.white;
        thumbnail.color = Color.white;
        quantityText.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 0.1f).SetUpdate(true);

        if (PlayerManager.instance.CurrentPoints < price) { shop.CannotBuyError(((uint)price)); return; }
        if (currentQuantity >= maxQuantity) { shop.ReachedMaximumError(((uint)currentQuantity), ((uint)maxQuantity)); return; }
        
        PlayerManager.instance.RemovePoints(((uint)price));
        shop.GenerateStructure(productName);

        shop.CloseShop();
        shop.OpenShop();
        
        Debug.Log("item purchased");
        return;
    }
}
