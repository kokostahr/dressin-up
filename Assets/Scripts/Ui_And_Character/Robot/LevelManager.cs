using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("AI PERSONALITY SETUP")]
    public GameObject easyAI;
    public GameObject mediumAI;
    public GameObject hardAI;

    private Ai_OutfitChanger activeAi;
    public RoundManager roundManager;


    //Method to select the levels
    public void OnLevelChosen(int difficulty)
    {
        SetAIByDifficulty(difficulty);
        roundManager.StartRound();
    }

    void SetAIByDifficulty(int difficulty)
    {
        //dISABLE Tehm all at first
        easyAI.SetActive(false);
        mediumAI.SetActive(false);
        hardAI.SetActive(false);

        //then activate only the one matching the selected difficulty
        switch (difficulty)
        {
            case 1: 
                easyAI.SetActive(true); 
                activeAi = easyAI.GetComponent<Ai_OutfitChanger>();
                break;
            case 2: 
                mediumAI.SetActive(true); 
                activeAi = mediumAI.GetComponent<Ai_OutfitChanger>();
                break;
            case 3: 
                hardAI.SetActive(true);
                activeAi = hardAI.GetComponent<Ai_OutfitChanger>();
                break;
        }

        if (activeAi != null && activeAi.clothingManager != null)
        {
            activeAi.clothingManager.SetAIDifficulty(difficulty); // ⭐ tell it everything
        }
    }

    public void StartAIRound()
    {
        if (activeAi != null)
        {
            StartCoroutine(activeAi.ChooseRandomOutfitDelay());
        }
        else
        {
            Debug.LogError("No active AI found when trying to start AI round!");
        }
    }
}
