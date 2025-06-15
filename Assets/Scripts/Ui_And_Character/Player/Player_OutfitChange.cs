using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_OutfitChange : MonoBehaviour
{
    // Arrays to hold the clothes for each category
    [Header("ARRAY CATERGORY")]
    public GameObject[] shirts;
    public GameObject[] pants;
    public GameObject[] shoes;

    //Variables to track the current worn item
    [HideInInspector] public GameObject currentShirt;
    [HideInInspector] public GameObject currentPants;
    [HideInInspector] public GameObject currentShoes;

    //Variables for the store items;
    public GameObject[] shopItems;

    // Panel for each category
    [Header("UI PANELS")]
    public GameObject shirtPanel, pantsPanel, shoePanel;

    // Image elements to display currently selected item
    [Header("DISPLAY IMAGES")]
    public Image shirtDisplay, pantsDisplay, shoeDisplay;

    // Sprites to represent the clothes in the UI
    [Header("UI SPRITE ARRAY")]
    public Sprite[] shirtSprites, pantsSprites, shoeSprites;

    [Header("REFERENCES")]
    //public TextMeshProUGUI playerLiveScoreText;
    public ClothingManager clothingManager;
    

    void Start()
    {
        // Hide everything at the start
        HideAll(shirts);
        HideAll(pants);
        HideAll(shoes);
        HideAll(shopItems);
        shirtPanel.SetActive(false);
        //Show the items that have already been bought
        RevealAlreadyBoughtItems();
    }

    // Function to hide all items in a category
    void HideAll(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
    }

    //method to just hide all the stuff when the round resets
    public void PlayerHideAllRoundReset()
    {
        HideAll(shirts);
        HideAll(pants);
        HideAll(shoes);
    }

    //Function to Reveal the items that have been bought, into the player's wardrobe
    public void RevealBoughtClothingUI (string itemName)
    {
        Transform itemUI = FindChildRecursive(shirtPanel.transform, itemName);
        if (itemUI == null)
            itemUI = FindChildRecursive(pantsPanel.transform, itemName);
        if (itemUI == null)
            itemUI = FindChildRecursive(shoePanel.transform, itemName);

        if (itemUI != null)
        {
            Debug.Log("Found item: " + itemUI.name); // Debug log to confirm the item is found
            itemUI.gameObject.SetActive(true); //then activate/show the relevant item inside the panel
            Debug.Log("Searching for: " + itemName); // See exactly what the name is when searching
        }
        else
        {
            Debug.Log("Item not found: " + itemName); // Debug log if item is not found
        }
    }

    //idk man
    Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }

    //A method that will reveal the clothing items that have already been bought in a specific play session.
    void RevealAlreadyBoughtItems()
    {
        string[] allPossibleItemNames = 
            {"FashionSneakers", "LongBoots", "FlowyPants", "FashionSweats", 
            "FashionShorts", "BaggyLowJeans", "Wind Jacket", "TopHoodie", 
            "DenimTop", "SweaterShirt", "FancyShirt"};

        foreach (string name  in allPossibleItemNames)
        {
            if (PlayerPrefs.GetInt("Bought_" +  name, 0) == 1)
            {
                RevealBoughtClothingUI(name);
            }
        }
    }

    //function that will do the random picking of clothes cards:
    public void GenerateRandomRoundItems()
    {
        Debug.Log("Generating the player round oufti items");
        //first hide everything
        PlayerHideAllRoundReset();

        //Now reveal the three random items of each category
        RevealRandomItems(shirts, shirtPanel, 3);
        RevealRandomItems(pants, pantsPanel, 3);
        RevealRandomItems(shoes, shoePanel, 3);

    }

    void RevealRandomItems(GameObject[] items, GameObject panel, int count)
    {
        List<int> chosenIndexes = new List<int>();
        int safeLimit = 50; // just in case you got low variety lol

        while (chosenIndexes.Count < count && safeLimit > 0)
        {
            int rand = Random.Range(0, items.Length);
            if (!chosenIndexes.Contains(rand))
                chosenIndexes.Add(rand);

            safeLimit--;
        }

        foreach (int index in chosenIndexes)
        {
            items[index].SetActive(true);
            // Also make sure their corresponding UI buttons inside the panel are active
            Transform ui = FindChildRecursive(panel.transform, items[index].name);
            if (ui != null)
                ui.gameObject.SetActive(true);
        }
    }

    // Functions to set specific clothes based on button click
    public void SetShirt(int index)
    {
        HideAll(shirts);
        if (index >= 0 && index < shirts.Length)
        {
            shirts[index].SetActive(true);
            currentShirt = shirts[index];
            if (index < shirtSprites.Length)
                shirtDisplay.sprite = shirtSprites[index];

            //Update the score for the shirt
            UpdatePlayerScore();

            // Trigger the bonus popup for the shirt
            ClothingItemData shirtData = shirts[index].GetComponent<ClothingItemHolder>().clothingItemData;
            string currentTheme = RoundManager.Instance.GetCurrentTheme();
            clothingManager.SelectClothingItem(shirtData, currentTheme);
        }
    }

    public void SetPants(int index)
    {
        HideAll(pants);
        if (index >= 0 && index < pants.Length)
        {
            pants[index].SetActive(true);
            currentPants = pants[index];
            if (index < pantsSprites.Length)
                pantsDisplay.sprite = pantsSprites[index];

            //Update the score for pants
            UpdatePlayerScore();

            // Trigger the bonus popup for the pants
            ClothingItemData pantsData = pants[index].GetComponent<ClothingItemHolder>().clothingItemData;
            string currentTheme = RoundManager.Instance.GetCurrentTheme();
            clothingManager.SelectClothingItem(pantsData, currentTheme);
        }
    }

    public void SetShoes(int index)
    {
        HideAll(shoes);
        if (index >= 0 && index < shoes.Length)
        {
            shoes[index].SetActive(true);
            currentShoes = shoes[index];
            if (index < shoeSprites.Length)
                shoeDisplay.sprite = shoeSprites[index];

            //Update the score for shoes
            UpdatePlayerScore();

            ClothingItemData shoesData = shoes[index].GetComponent<ClothingItemHolder>().clothingItemData;
            string currentTheme = RoundManager.Instance.GetCurrentTheme();
            clothingManager.SelectClothingItem(shoesData, currentTheme);
        }
    }

    //Method to update player's score after they select a clothing piece
    void UpdatePlayerScore()
    {
        string currentTheme = RoundManager.Instance.GetCurrentTheme();
        int playerScore = clothingManager.CalculateOutfitScore(currentTheme);

        //Update the ui 
        clothingManager.UpdatePlayerScore(playerScore);
    }

    //Disabling and Enabling the relevant panels when it is a player's turn
    //lock the panels:
    public void DisablePlayerInput()
    {
        Debug.Log("Locking player wardrobe");
        shirtPanel.SetActive(false);
        pantsPanel.SetActive(false);
        shoePanel.SetActive(false);

        // Optional: You can also gray out or disable specific buttons if you want a UI effect
        //Game object that will be active and say "WAIT UR TURN"
    }

    //UNLOCK THE PANEL. LET THEM COOK
    public void EnableInput()
    {
        Debug.Log("Unlocking player wardrobe");
        shirtPanel.SetActive(true);
        pantsPanel.SetActive(true);
        shoePanel.SetActive(true);
    }

    // Panel Open/Close Methods for each category
    public void OpenShirtPanel()
    {
        shirtPanel.SetActive(true);
    }

    public void CloseShirtPanel()
    {
        shirtPanel.SetActive(false);
    }

    public void OpenPantsPanel()
    {
        pantsPanel.SetActive(true);
    }

    public void ClosePantsPanel()
    {
        pantsPanel.SetActive(false);
    }

    public void OpenShoePanel()
    {
        shoePanel.SetActive(true);
    }

    public void CloseShoePanel()
    {
        shoePanel.SetActive(false);
    }
}
////Function to update the player's score live
//void UpdatePlayerScore()
//{
//    //Calculate the player's current outfit score based on the selected items
//    int totalScore = 0;

//    if (currentShirt != null)
//    {
//        totalScore += ClothingManager.CalculateOutfitScore(currentShirt);
//    } 
//}