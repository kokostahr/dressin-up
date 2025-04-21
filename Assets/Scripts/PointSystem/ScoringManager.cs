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

        //Checking which items are currently equiped for the player
        ClothingItemData[] equippedItems = new ClothingItemData[3];

        if (clothingManager.playerOutfitChanger.currentShirt != null)
        {
            equippedItems[0] = clothingManager.playerOutfitChanger.currentShirt?.GetComponent<ClothingItemHolder>()?.clothingItemData;
            equippedItems[1] = clothingManager.playerOutfitChanger.currentPants?.GetComponent<ClothingItemHolder>()?.clothingItemData;
            equippedItems[2] = clothingManager.playerOutfitChanger.currentShoes?.GetComponent<ClothingItemHolder>()?.clothingItemData;

        }

        //Get the bonus points
        int bonus = PlayerBonus(equippedItems, theme);

        //combine the bonus points with the normal points
        playerTotalScore = playerScore + bonus;


        return playerTotalScore;
    }

    //Getting the updated scores for the ai player
    public int UpdateAIScore(string theme)
    {
        int aiScore = clothingManager.CalculateAiOutfitScore(theme);

        //Checking which items are currently eqquiped for the AI
        ClothingItemData[] equippedItems = new ClothingItemData[3];

        if (clothingManager.aiOutfitChanger.currentShirt != null)
        {
            equippedItems[0] = clothingManager.aiOutfitChanger.currentShirt?.GetComponent<ClothingItemHolder>()?.clothingItemData;
            equippedItems[1] = clothingManager.aiOutfitChanger.currentPants?.GetComponent<ClothingItemHolder>()?.clothingItemData;
            equippedItems[2] = clothingManager.aiOutfitChanger.currentShoes?.GetComponent<ClothingItemHolder>()?.clothingItemData;

        }

        //Get the bonus points 
        int bonus = AiBonusPoints(equippedItems, theme);

        //combine the bonus to the actual score
        aiTotalScore = aiScore + bonus;


        return aiTotalScore;

    }


    //Setting up the bonus points for the player
    public int PlayerBonus(ClothingItemData[] equippedItems, string theme)
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
                //WINTER
                if (theme == "winter" && (tag == "cozy" || tag == "warm"))
                {
                    //ADD BONUS POINTS TO THE INITIAL SCORE
                    bonusScore += 2;
                }

                if (theme == "winter" && (tag == "chic" || tag == "warm"))
                {
                    //Add bonus points
                    bonusScore += 3;
                }

                if (theme == "winter" && (tag == "bold" || tag == "edgy"))
                {
                    //Add bonus points
                    bonusScore += 1;
                }

                if (theme == "winter" && (tag == "flirty"))//Seperated them so bonus points can stack. 
                    //If i added them all into one line like flirty || vibrant || etc, then its either or
                    //not you can have flirty bonus points AND vibrant bonus points. 
                {
                    //Add bonus points
                    bonusScore += 1;
                }

                if (theme == "winter" && (tag == "vibrant"))
                {
                    //Add bonus points
                    bonusScore += 1;
                }

                //SUMMER
                if (theme == "summer" && (tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "summer" && (tag == "flirty"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "summer" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "summer" && (tag == "bold" || tag == "edgy" || tag == "chic"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                //CASUAL DATE
                if (theme == "casualdate" && (tag == "flirty" || tag == "chic"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "casualdate" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "casualdate" && (tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "casualdate" && (tag == "warm"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                //ARTSY STREET STYLE
                if (theme == "artstreetstyle" && (tag == "edgy" || tag == "chic" || tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "artstreetstyle" && (tag == "bold"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "artstreetstyle" && (tag == "flirty"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "artstreetstyle" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                //TV SHOW AUDITION

                if (theme == "tvshowaudition" && (tag == "bold" || tag == "chic"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "tvshowaudition" && (tag == "edgy" || tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "tvshowaudition" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "tvshowaudition" && (tag == "warm"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }
                Debug.Log("Player Bonus Score: " + bonusScore);
            }
        }

        //then add the bonus score to the player's total score
        lastPlayerBonus = bonusScore;
        playerTotalScore += bonusScore;

        return bonusScore;

        //add a text popup or sparkle here 
    }

    public int AiBonusPoints(ClothingItemData[] equippedItems, string theme)
    {
        //Defining what the bonus score is so that we can ADD IT to the player's final score at the end of a round
        int bonusScore = 0;

        //Code that will look at the tags an item has
        foreach (ClothingItemData item in equippedItems)
        {
            //then need to check through the different tags an item will have
            foreach (string tag in item.itemTag)
            {
                //WINTER
                if (theme == "winter" && (tag == "cozy" || tag == "warm"))
                {
                    //ADD BONUS POINTS TO THE INITIAL SCORE
                    bonusScore += 2;
                }

                if (theme == "winter" && (tag == "chic" || tag == "warm"))
                {
                    //Add bonus points
                    bonusScore += 3;
                }

                if (theme == "winter" && (tag == "bold" || tag == "edgy"))
                {
                    //Add bonus points
                    bonusScore += 1;
                }

                if (theme == "winter" && (tag == "flirty"))//Seperated them so bonus points can stack. 
                                                           //If i added them all into one line like flirty || vibrant || etc, then its either or
                                                           //not you can have flirty bonus points AND vibrant bonus points. 
                {
                    //Add bonus points
                    bonusScore += 1;
                }

                if (theme == "winter" && (tag == "vibrant"))
                {
                    //Add bonus points
                    bonusScore += 1;
                }

                //SUMMER
                if (theme == "summer" && (tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "summer" && (tag == "flirty"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "summer" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "summer" && (tag == "bold" || tag == "edgy" || tag == "chic"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                //CASUAL DATE
                if (theme == "casualdate" && (tag == "flirty" || tag == "chic"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "casualdate" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "casualdate" && (tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "casualdate" && (tag == "warm"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                //ARTSY STREET STYLE
                if (theme == "artstreetstyle" && (tag == "edgy" || tag == "chic" || tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "artstreetstyle" && (tag == "bold"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "artstreetstyle" && (tag == "flirty"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "artstreetstyle" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                //TV SHOW AUDITION

                if (theme == "tvshowaudition" && (tag == "bold" || tag == "chic"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 3;
                }

                if (theme == "tvshowaudition" && (tag == "edgy" || tag == "vibrant"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 2;
                }

                if (theme == "tvshowaudition" && (tag == "cozy"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }

                if (theme == "tvshowaudition" && (tag == "warm"))
                {
                    //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
                    bonusScore += 1;
                }
            }
        }

        //then add the bonus score to the player's total score
        lastAIBonus = bonusScore;
        aiTotalScore += bonusScore;

        return bonusScore;

        //add a text popup or sparkle here 
    }

}
