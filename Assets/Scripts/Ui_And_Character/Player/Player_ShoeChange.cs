using UnityEngine;
using UnityEngine.UI;

public class Player_ShoeChange : MonoBehaviour
{
    #region First Attempt
    //public GameObject shoePanel;
    ////Array of all the shoe game objects
    //public GameObject[] shoes;
    ////Integer to track which shoe is currently active
    //public int currentShoeIndex = -1;
    ////Need a variable that will display the currently worn item when the menu is closed
    ////public Image shoeDisplayImage;

    //void Start()
    //{
    //    //Ensure only the default shoe is visible at the start
    //    //UpdateShoeDisplay();
    //}

    ////fUNCTION that will open the panel when the button is pressed
    //public void OpenShoePanel()
    //{
    //    shoePanel.SetActive(true);
    //    //Panel will be visible
    //}

    ////Function that will close the shoe panel when the button is pressed
    //public void CloseShoePanel()
    //{
    //    shoePanel.SetActive(false);
    //    //Panel will not be visible
    //}

    ////Since Im using image buttons, need various functions that will update and display the relevant item clicked on
    ////public void ChangeBoots() {whatShoe = 1}

    ////function to change to the relevant shoe






    //Need code that puts all the items in an index, and constantly updates to check which item in the array has been pressed
    //void Update()
    //{
    //    //need a line of code that sets the display of the shoeDisplayImage to the current displayed image
    //    //some pseudo code cuz idk if this is right
    //    //if (whatShoe == 1) 
    //    //{
    //    // Set it to the relevant game object that will be displayed.
    //    //}
        
    //}
    #endregion
    #region Second Attempt

    public GameObject shoePanel;
    //Array of all the shoe game objects
    public GameObject[] shoes;

    //The shoe display image that displayes the currently worn shoe
    public Image shoeDisplay;
    //An array to store spirtes that correspond to each shoe
    public Sprite[] shoeSprites;

    void Start()
    {
        //Ensure only the default shoe is visible at the start
        HideAllShoes();
    }

    //fUNCTION that will open the panel when the button is pressed
    public void OpenShoePanel()
    {
        shoePanel.SetActive(true);
        //Panel will be visible
    }

    //Function that will close the shoe panel when the button is pressed
    public void CloseShoePanel()
    {
        shoePanel.SetActive(false);
        //Panel will not be visible
    }
    void HideAllShoes()
    {
        foreach (GameObject shoe in shoes)
        {
            //hide all the shoes
            shoe.SetActive(false);
        }
    }

    public void SetShoe(int shoeIndex)
    {
        HideAllShoes();
        if (shoeIndex >= 0 && shoeIndex < shoes.Length)
        {
            //Show the relevant shoe
            shoes[shoeIndex].SetActive(true);

            //Update the shoe display image
            if (shoeSprites != null && shoeIndex < shoeSprites.Length)
            {
                shoeDisplay.sprite = shoeSprites[shoeIndex];
            }
        }
    }

    #endregion
}
