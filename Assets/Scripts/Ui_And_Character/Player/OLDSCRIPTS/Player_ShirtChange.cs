using UnityEngine;
using UnityEngine.UI;

public class Player_ShirtChange : MonoBehaviour
{
    //SELF EXPLANATORY. GameObject for the panel
    public GameObject shirtPanel;

    //Array of all the pants game objects
    public GameObject[] shirts;

    //The shoe display image that displayes the currently worn shoe
    public Image shirtDisplay;
    //An array to store spirtes that correspond to each shoe
    public Sprite[] shirtSprites;

    void Start()
    {
        //Ensure only the default shoe is visible at the start
        HideAllShirts();
    }


    //fUNCTION that will open the panel when the button is pressed
    public void OpenShirtPanel()
    {
        shirtPanel.SetActive(true);
        //Panel will be visible

    }

    public void CloseShirtPanel()
    {
        shirtPanel.SetActive(false);
        //Panel will not be visible
    }

    void HideAllShirts()
    {
        foreach (GameObject shirt in shirts)
        {
            //hide all the shoes
            shirt.SetActive(false);
        }
    }

    public void SetShirt(int shirtIndex)
    {
        HideAllShirts();
        if (shirtIndex >= 0 && shirtIndex < shirts.Length)
        {
            //Show the relevant shoe
            shirts[shirtIndex].SetActive(true);

            //Update the shoe display image
            if (shirtSprites != null && shirtIndex < shirtSprites.Length)
            {
                shirtDisplay.sprite = shirtSprites[shirtIndex];
            }
        }
    }

}
