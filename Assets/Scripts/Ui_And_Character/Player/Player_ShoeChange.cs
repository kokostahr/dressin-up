using UnityEngine;

public class Player_ShoeChange : MonoBehaviour
{
    public GameObject shoePanel;



    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //fUNCTION that will open the panel when the button is pressed
    public void OpenShoePanel()
    {
        shoePanel.SetActive(true);
        //Panel will be visible

    }

    public void CloseShoePanel()
    {
        shoePanel.SetActive(false);
        //Panel will not be visible
    }

}
