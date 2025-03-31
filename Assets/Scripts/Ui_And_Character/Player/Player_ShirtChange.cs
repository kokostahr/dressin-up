using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player_ShirtChange : MonoBehaviour
{
    //SELF EXPLANATORY. GameObject for the panel
    public GameObject shirtPanel;

    void Start()
    {
        
    }

    
    void Update()
    {
        
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
}
