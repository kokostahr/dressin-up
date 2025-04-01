using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class Player_PantsChange : MonoBehaviour
{

    public GameObject pantPanel;

    //Array of all the pants game objects
    public GameObject[] pants;

    //The shoe display image that displayes the currently worn shoe
    public Image pantDisplay;
    //An array to store spirtes that correspond to each shoe
    public Sprite[] pantSprites;

    void Start()
    {
        //Ensure only the default shoe is visible at the start
        HideAllPants();
    }

    //fUNCTION that will open the panel when the button is pressed
    public void OpenPantsPanel()
    {
        pantPanel.SetActive(true);
        //Panel will be visible

    }

    public void ClosePantsPanel()
    {
        pantPanel.SetActive(false);
        //Panel will not be visible
    }

    void HideAllPants()
    {
        foreach (GameObject pant in pants)
        {
        //hide all the shoes
        pant.SetActive(false);
        }
    }

    public void SetPant(int pantIndex)
    {
        HideAllPants();
        if (pantIndex >= 0 && pantIndex < pants.Length)
        {
            //Show the relevant shoe
            pants[pantIndex].SetActive(true);

            //Update the shoe display image
            if (pantSprites != null && pantIndex < pantSprites.Length)
            {
                pantDisplay.sprite = pantSprites[pantIndex];
            }
        }
    }

}
