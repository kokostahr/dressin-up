using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingManager : MonoBehaviour
{
    //May not be necessary, but lets see
    //public Image clothingDisplay;
    //Text to show point popup when clothes are selected
    public TextMeshProUGUI pointsPopupText;
    //Int to track player's score
    public int totalWinterPoints = 0;
    public Player_OutfitChange playerOutfitChanger;

    //Int to track the ai's score

    public Ai_OutfitChanger aiOutfitChanger;

    public void SelectClothingItem(ClothingItemData selectedItem)
    {
        //Update the UI display
        //clothingDisplay.sprite = selectedItem.itemIcon;

        //Update the player's score
        totalWinterPoints += selectedItem.winterPoints;

        //Show the popup points
        ShowPointsPopup(selectedItem.winterPoints);
    }

    void ShowPointsPopup(int points)
    {
        pointsPopupText.text = "+" + points.ToString() + " points";
        pointsPopupText.gameObject.SetActive(true);
        StartCoroutine(HidePointsPopup());
    }

    IEnumerator HidePointsPopup()
    {
        yield return new WaitForSeconds(1f);
        pointsPopupText.gameObject.SetActive(false);
    }

    //Need a method that will calculate the points of the current clothing worn
    public int CalculateOutfitScore()
    {
        int score = 0;

        //Let's try and get ClothingItemData from each equipped piece
        if (playerOutfitChanger.currentShirt != null)
        {
            var data = playerOutfitChanger.currentShirt.GetComponent<ClothingItemHolder>();   
            if (data != null)
            {
                score += data.clothingItemData.winterPoints;
            }
        }

        if (playerOutfitChanger.currentPants != null)
        {
            var data = playerOutfitChanger.currentPants.GetComponent<ClothingItemHolder>();
            if (data != null) score += data.clothingItemData.winterPoints;
        }

        if (playerOutfitChanger.currentShoes != null)
        {
            var data = playerOutfitChanger.currentShoes.GetComponent<ClothingItemHolder>();
            if (data != null) score += data.clothingItemData.winterPoints;
        }

        return score;
    }

    public int CalculateAiOutfitScore()
    {
        int score = 0;

        if (aiOutfitChanger.currentShirt != null)
        {
            var data = aiOutfitChanger.currentShirt.GetComponent<ClothingItemHolder>();
            if (data != null) score += data.clothingItemData.winterPoints;
        }

        if (aiOutfitChanger.currentPants !=null)
        {
            var data = aiOutfitChanger.currentPants.GetComponent<ClothingItemHolder>();
            if (data != null) score += data.clothingItemData.winterPoints;
        }
        
        if (aiOutfitChanger.currentShoes!= null)
        {
            var data = aiOutfitChanger.currentShoes.GetComponent<ClothingItemHolder>();
            if (data != null) score += data.clothingItemData.winterPoints;
        }
        return score;
    }

}


