using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    //Moving scoring and score comparison logic to this script to make expansion of the game easier
    //Refereincing the round manager, since the basic scoring is done there anyways.
    public RoundManager roundManager;
    public ClothingManager clothingManager;

    public static ScoringManager Instance;

    public int playerTotalScore = 0;
    public int aiTotalScore = 0;

    //INTS that we can reference to add the final bonus points to the player score
    public int lastPlayerBonus = 0;
    public int lastAIBonus = 0;
    //making a text that will popup when the player gets a bonus point


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

  //Getting the updated scores for the player
    public int UpdatePlayerScore(string theme)
    {
        int playerScore = clothingManager.CalculateOutfitScore(theme);

        playerTotalScore = playerScore;

        //Checking which items are currently equiped for the player
        ClothingItemData[] equippedItems = new ClothingItemData[3];

        equippedItems[0] = clothingManager.playerOutfitChanger.currentShirt?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        equippedItems[1] = clothingManager.playerOutfitChanger.currentPants?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        equippedItems[2] = clothingManager.playerOutfitChanger.currentShoes?.GetComponent<ClothingItemHolder>()?.clothingItemData;

        //Giving the player her bonus points 
        PlayerBonus(equippedItems, theme);

        return playerTotalScore;
    }

    //Getting the updated scores for the ai player
    public int UpdateAIScore(string theme)
    {
        int aiScore = clothingManager.CalculateAiOutfitScore(theme);

        aiTotalScore = aiScore;

        //Checking which items are currently eqquiped for the AI
        ClothingItemData[] equippedItems = new ClothingItemData[3];

        equippedItems[0] = clothingManager.aiOutfitChanger.currentShirt?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        equippedItems[1] = clothingManager.aiOutfitChanger.currentPants?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        equippedItems[2] = clothingManager.aiOutfitChanger.currentShoes?.GetComponent<ClothingItemHolder>()?.clothingItemData;

        //Giving the player her bonus points 
        AiBonusPoints(equippedItems, theme);

        return aiTotalScore;

    }


    //Setting up the bonus points for the player
    public void PlayerBonus(ClothingItemData[] equippedItems, string theme)
    {

        //Defining what the bonus score is so that we can ADD IT to the player's final score at the end of a round
        int bonusScore = 0;

        //Code that will look at the tags an item has
        foreach (ClothingItemData item in equippedItems)
        {
            //then need to check through the different tags an item will have
            foreach (string tag in item.itemTag)
            {
                //DEFINING THE BONUS CONDITIONS BASED ON BOTH THE THEME AND TAGS
                if (theme == "winter" && (tag == "cozy" || tag == "comfy"))
                {
                    //ADD BONUS POINTS TO THE INITIAL SCORE
                    bonusScore += 2; 
                }

                if (theme == "winter" && (tag == "chic" && tag == "warm"))
                {
                    //Add bonus points
                    bonusScore += 3;
                }

                if (theme == "summer" && tag == "vibrant")
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                //add the other conditions for the other themes!
            }
        }

        //then add the bonus score to the player's total score
        lastPlayerBonus = bonusScore;
        playerTotalScore += bonusScore;

        //add a text popup or sparkle here 
    }

    public void AiBonusPoints(ClothingItemData[] equippedItems, string theme)
    {
        //Defining what the bonus score is so that we can ADD IT to the player's final score at the end of a round
        int bonusScore = 0;

        //Code that will look at the tags an item has
        foreach (ClothingItemData item in equippedItems)
        {
            //then need to check through the different tags an item will have
            foreach (string tag in item.itemTag)
            {
                //DEFINING THE BONUS CONDITIONS BASED ON BOTH THE THEME AND TAGS
                if (theme == "winter" && (tag == "cozy" || tag == "comfy"))
                {
                    //ADD BONUS POINTS TO THE INITIAL SCORE
                    bonusScore += 2;
                }

                if (theme == "winter" && (tag == "chic" && tag == "warm"))
                {
                    //Add bonus points
                    bonusScore += 3;
                }

                if (theme == "summer" && tag == "vibrant")
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }
            }
        }

        //then add the bonus score to the player's total score
        lastAIBonus = bonusScore;
        aiTotalScore += bonusScore;

        //add a text popup or sparkle here 
    }

}
