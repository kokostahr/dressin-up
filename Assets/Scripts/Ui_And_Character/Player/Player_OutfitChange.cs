using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
