using System.Linq;
using TMPro;
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
        int baseScore = clothingManager.CalculateOutfitScore(theme);
        int bonus = PlayerBonus(theme);

        playerTotalScore = baseScore + bonus;
        return playerTotalScore;
    }

    //Getting the updated scores for the ai player
    public int UpdateAIScore(string theme)
    {
        // Get the AI's total base score from their collected items
        int aiScore = clothingManager.CalculateAiOutfitScore(theme);

        // Get the AI's bonus points based on their collected items
        int bonus = AiBonusPoints(clothingManager.aiCollectedItems.ToArray(), theme);

        aiTotalScore = aiScore + bonus;

        return aiTotalScore;
    }

    //Setting up the bonus points for the player
    public int PlayerBonus(string theme)
    {
        int bonusScore = 0;
        ClothingItemData[] equippedItems = clothingManager.playerCollectedItems.ToArray();

        // Matching tags check
        if (equippedItems.Length >= 3)
        {
            for (int i = 0; i < equippedItems.Length - 2; i++)
            {
                foreach (string tag in equippedItems[i].itemTag)
                {
                    if (equippedItems[i + 1].itemTag.Contains(tag) && equippedItems[i + 2].itemTag.Contains(tag))
                    {
                        bonusScore += 2;
                        Debug.Log("⭐ PLAYER MATCHING TAG BONUS: " + tag);
                    }
                }
            }
        }

        // Theme-specific bonuses
        foreach (ClothingItemData item in equippedItems)
        {
            bonusScore += GetBonusForSingleItem(item, theme);
        }

        lastPlayerBonus = bonusScore;
        return bonusScore;
    }

    //method to calculaye the bonus score for each item, on its own
    public int GetBonusForSingleItem(ClothingItemData item, string theme)
    {
        int bonus = 0;

        if (item == null || item.itemTag == null)
        {
            return 0;
        }

        foreach (string tag in item.itemTag)
        {
            //WINTER THEMEEEEEE
            if (theme == "winter")
            {
                if (tag == "cozy" || tag == "warm") {bonus += 2;}
                if (tag == "chic") bonus += 3;
                if (tag == "bold" || tag == "edgy") bonus += 1;
                if (tag == "flirty") bonus += 1;
                if (tag == "vibrant") bonus += 1;
            }

            //SUMMER THEME
            else if (theme == "summer")
            {
                if (tag == "vibrant") bonus += 3;
                if (tag == "flirty") bonus += 2;
                if (tag == "cozy") bonus += 1;
                if (tag == "bold" || tag == "edgy" || tag == "chic") bonus += 1;
            }

            //CASUAL DATE THEME
            else if (theme == "casual date")
            {
                if (tag == "flirty" || tag == "chic") bonus += 3;
                if (tag == "cozy") bonus += 2;
                if (tag == "vibrant") bonus += 1;
                if (tag == "warm") bonus += 1;
            }

            //ARTSY STREET STYLE THEME
            else if (theme == "artsy street style")
            {
                if (tag == "edgy" || tag == "chic" || tag == "vibrant") bonus += 3;
                if (tag == "bold") bonus += 2;
                if (tag == "flirty") bonus += 1;
                if (tag == "cozy") bonus += 1;
            }

            //TV SHOW AUDITION
            else if (theme == "tv show audition")
            {
                if (tag == "bold" || tag == "chic") bonus += 3;
                if (tag == "edgy" || tag == "vibrant") bonus += 2;
                if (tag == "cozy") bonus += 1;
                if (tag == "warm") bonus += 1;
            }
        }

        return bonus;
    }

    public int AiBonusPoints(ClothingItemData[] collectedItems, string theme)
    {
        int bonusScore = 0;

        // Matching tags check (at least 3 items needed)
        if (collectedItems.Length >= 3)
        {
            for (int i = 0; i < collectedItems.Length - 2; i++)
            {
                foreach (string tag in collectedItems[i].itemTag)
                {
                    if (collectedItems[i + 1].itemTag.Contains(tag) && collectedItems[i + 2].itemTag.Contains(tag))
                    {
                        bonusScore += 2;
                        Debug.Log("🤖 AI MATCHING TAG BONUS: " + tag);
                    }
                }
            }
        }

        // Theme-specific bonuses per item
        foreach (ClothingItemData item in collectedItems)
        {
            bonusScore += GetBonusForSingleItem(item, theme);
        }

        lastAIBonus = bonusScore;
        return bonusScore;
    }
}

//OLD BONUS SCORING LOGIC THAT WAS INSIDE PLAYERBONUS()
////Code that will look at the tags an item has
//foreach (ClothingItemData item in equippedItems)
//{
//    //then need to check through the different tags an item will have
//    foreach (string tag in item.itemTag)
//    {
//        //DEFINING THE BONUS CONDITIONS BASED ON BOTH THE THEME AND TAGS
//        //WINTER
//        if (theme == "winter" && (tag == "cozy" || tag == "warm"))
//        {
//            //ADD BONUS POINTS TO THE INITIAL SCORE
//            bonusScore += 2;
//        }

//        if (theme == "winter" && (tag == "chic"))
//        {
//            //Add bonus points
//            bonusScore += 3;
//        }

//        if (theme == "winter" && (tag == "bold" || tag == "edgy"))
//        {
//            //Add bonus points
//            bonusScore += 1;
//        }

//        if (theme == "winter" && (tag == "flirty"))//Seperated them so bonus points can stack. 
//            //If i added them all into one line like flirty || vibrant || etc, then its either or
//            //not you can have flirty bonus points AND vibrant bonus points. 
//        {
//            //Add bonus points
//            bonusScore += 1;
//        }

//        if (theme == "winter" && (tag == "vibrant"))
//        {
//            //Add bonus points
//            bonusScore += 1;
//        }

//        //SUMMER
//        if (theme == "summer" && (tag == "vibrant"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 3;
//        }

//        if (theme == "summer" && (tag == "flirty"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 2;
//        }

//        if (theme == "summer" && (tag == "cozy"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        if (theme == "summer" && (tag == "bold" || tag == "edgy" || tag == "chic"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        //CASUAL DATE
//        if (theme == "casualdate" && (tag == "flirty" || tag == "chic"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 3;
//        }

//        if (theme == "casualdate" && (tag == "cozy"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 2;
//        }

//        if (theme == "casualdate" && (tag == "vibrant"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        if (theme == "casualdate" && (tag == "warm"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        //ARTSY STREET STYLE
//        if (theme == "artstreetstyle" && (tag == "edgy" || tag == "chic" || tag == "vibrant"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 3;
//        }

//        if (theme == "artstreetstyle" && (tag == "bold"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 2;
//        }

//        if (theme == "artstreetstyle" && (tag == "flirty"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        if (theme == "artstreetstyle" && (tag == "cozy"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        //TV SHOW AUDITION

//        if (theme == "tvshowaudition" && (tag == "bold" || tag == "chic"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 3;
//        }

//        if (theme == "tvshowaudition" && (tag == "edgy" || tag == "vibrant"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 2;
//        }

//        if (theme == "tvshowaudition" && (tag == "cozy"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }

//        if (theme == "tvshowaudition" && (tag == "warm"))
//        {
//            //ADD THREE BONUS POINTS TO THE INITIAL SCORRRRRE
//            bonusScore += 1;
//        }
//        Debug.Log("Player Bonus Score: " + bonusScore);
//    }
//}
