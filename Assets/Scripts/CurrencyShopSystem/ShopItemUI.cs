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
        costText.text = item.cost.ToString();

        //Ensuring that it doesn't mess with the boolean, because if they just click it, doesn't mean they necessairly bought it
        buyButton.interactable = !item.isBought;
        buyButton.onClick.AddListener(BuyItem);

    }

    //Method that will work if the player clicks the appropriate thing to buy clothes
    void BuyItem()
    {
        if (PlayerCurrency.Instance.SpendPoints(myItem.cost))
        {
            //If the player manages to buy an item, then set the bool to true, make item disappear from the shop!
            myItem.isBought = true;
            buyButton.interactable = false; //THEY SHOUDN'T BE ABLE TO CLICK AND BUY INFINITE AMOUNT OF 1 THING

            //IF I HAVE TIME TO STORE THE BOUGHT DATA (like what they have and havent bought in each play session:
            PlayerPrefs.SetInt("Bought_" + myItem.itemName, 1);
        }
    }
}
