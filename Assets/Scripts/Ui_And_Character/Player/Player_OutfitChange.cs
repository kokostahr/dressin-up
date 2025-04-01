using UnityEngine;
using UnityEngine.UI;

public class Player_OutfitChange : MonoBehaviour
{
    // Arrays to hold the clothes for each category
    [Header("ARRAY CATERGORY")]
    public GameObject[] shirts;
    public GameObject[] pants;
    public GameObject[] shoes;

    // Panel for each category
    [Header("UI PANELS")]
    public GameObject shirtPanel, pantsPanel, shoePanel;

    // Image elements to display currently selected item
    [Header("DISPLAY IMAGES")]
    public Image shirtDisplay, pantsDisplay, shoeDisplay;

    // Sprites to represent the clothes in the UI
    [Header("UI SPRITE ARRAY")]
    public Sprite[] shirtSprites, pantsSprites, shoeSprites;

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

    // Functions to set specific clothes based on button click
    public void SetShirt(int index)
    {
        HideAll(shirts);
        if (index >= 0 && index < shirts.Length)
        {
            shirts[index].SetActive(true);
            if (index < shirtSprites.Length)
                shirtDisplay.sprite = shirtSprites[index];
        }
    }

    public void SetPants(int index)
    {
        HideAll(pants);
        if (index >= 0 && index < pants.Length)
        {
            pants[index].SetActive(true);
            if (index < pantsSprites.Length)
                pantsDisplay.sprite = pantsSprites[index];
        }
    }

    public void SetShoes(int index)
    {
        HideAll(shoes);
        if (index >= 0 && index < shoes.Length)
        {
            shoes[index].SetActive(true);
            if (index < shoeSprites.Length)
                shoeDisplay.sprite = shoeSprites[index];
        }
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
