using UnityEngine;

public class LevelSelect : MonoBehaviour
{
   

    [Header("LEVEL ACCESSIBILITY SETUP")]
    public LevelManager levelManager;
    public RoundManager roundManager;
    public GameObject level2Button;
    public GameObject level3Button;

  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level2Button.SetActive(false);
        level3Button.SetActive(false);

        //just call this this method at the beginning of the level select menu:
        RevealLevelButtons();
    }

    public void RevealLevelButtons()
    {
        if (roundManager.playerWins <= 2)
        {
            level2Button.SetActive(true);
            level3Button.SetActive(true);
        }
        //else if (roundManager.playerWins <= 4)
        //{

        //}
    }
}
