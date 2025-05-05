using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopItemUI : MonoBehaviour
{
    [Header("UI SETUP FOR EACH ITEM")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Button buyButton;

    ClothingItemData myItem;

    //Method that will set up each item appropriately within the shop 
    public void SetUp(ClothingItemData item)
    {
        myItem = item;
        icon.sprite = item.itemIcon;
        nameText.text = item.itemName;
        costText.text = item.cost.ToString() + " pts";

        //Ensuring that it doesn't mess with the boolean, because if they just click it, doesn't mean they necessairly bought it
        bool isBought = PlayerClothingData.IsItemBought(item.itemName);
        buyButton.interactable = !isBought;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

    }

    //Method that will work if the player clicks the appropriate thing to buy clothes
    void BuyItem()
    {
        if (PlayerCurrency.Instance.SpendPoints(myItem.cost))
        {
            //If the player manages to buy an item, then set the bool to true, make item disappear from the shop!
            PlayerClothingData.MarkItemAsBought(myItem.itemName);
            buyButton.interactable = false;

            // Then unlock the wardrobe icon
            Object.FindFirstObjectByType<Player_OutfitChange>().RevealBoughtClothingUI(myItem.itemName);
        }
    }
}
