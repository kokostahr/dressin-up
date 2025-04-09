using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject howToPlayPanel;

    private void Start()
    {
        howToPlayPanel.SetActive(false);
    }

    public void LoadScene(string sceneName) //To load the relevant scene when you click the button.
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() //To exit the apllication when you click the button
    {
        Application.Quit();
    }

    public void OpenInstructionPanel()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseInstructionPanel()
    {
        howToPlayPanel.SetActive(false);
    }

}