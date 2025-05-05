using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine.Rendering.VirtualTexturing;

public class ShopManager : MonoBehaviour
{

    //SCRIPT THAT WILL HANDLE ALL THE ANNOYING SHOP STUFF
    [Header("Shop SetUp")]
    //The UI prefab for each item in the shop
    public GameObject itemPrefab;
    //A panel that will display the items 
    public Transform contentPanel;
    //A list of all the avaliable clothes for purchase
    public List<ClothingItemData> itemsForSale;
    //shop panel gameobject
    public GameObject shopPanel;

    //public PlayerClothingData playerClothingData;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //When the game starts, populate the shop with the avaliable items
        PopulateShop();
        shopPanel.SetActive(false);
        itemPrefab.SetActive(false);

        //remove the player prefs when the game starts so i can playtest
        PlayerClothingData.ResetAllPurchasesForDebug();


    }

    void PopulateShop()
    {
        foreach (ClothingItemData item in itemsForSale)
        {
            GameObject newItem = Instantiate(itemPrefab, contentPanel);
            ShopItemUI ui = newItem.GetComponent<ShopItemUI>();
            ui.SetUp(item);

        }
    }

    //method that will open the shop panel. Lmao
    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    //method that will close the shop panel
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
