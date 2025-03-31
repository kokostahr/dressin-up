using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player_PantsChange : MonoBehaviour
{

    public GameObject pantPanel;

    void Start()
    {
        
    }

    
    void Update()
    {
        
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

}
