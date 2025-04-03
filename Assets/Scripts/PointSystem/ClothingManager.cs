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
        pointsPopupText.text = "+" + points.ToString() + "points";
        pointsPopupText.gameObject.SetActive(true);
        StartCoroutine(HidePointsPopup());
    }

    IEnumerator HidePointsPopup()
    {
        yield return new WaitForSeconds(2f);
        pointsPopupText.gameObject.SetActive(false);
    }

}
