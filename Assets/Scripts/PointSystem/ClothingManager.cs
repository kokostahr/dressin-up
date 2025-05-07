using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using DentedPixel;

public class ClothingManager : MonoBehaviour
{
    [Header("PLAYER RELATED SCORING")]
    //Text to show point popup when clothes are selected
    public TextMeshProUGUI pointsPopupText;
    //Int to track player's score
    int playerScore = 0;
    int aiScore = 0;
    //public int totalWinterPoints = 0;
    public int totalPoints = 0;
    //variale to track the player's live score
    //public int currentScore = 0;
    public Player_OutfitChange playerOutfitChanger;

    [Header("AI RELATED SCORING")]
    //Int to track the ai's score
    public Ai_OutfitChanger aiOutfitChanger;

    [Header("AI PERSONALITY SETUP")]
    private int aiDifficultyLevel = 1; //default easy

    public Lvl1_Ai_OutfitChanger wanaiOutfitChanger;
    public Lvl2_Ai_OutfitChanger tooaiOutfitChanger;
    public Lvl3_Ai_OutfitChanger treeaiOutfitChanger;

    [Header("LIVE SCOREBOARD UI")]
    //ui to track the live score of the player and Ai
    public TextMeshProUGUI playerLiveScoreText;
    public TextMeshProUGUI aiLiveScoreText;

    [Header("FINAL RESULTS UI")]
    public TextMeshProUGUI playerFinalScoreText;
    public TextMeshProUGUI aiFinalScoreText;

    [Header("BONUS POINTS LIVE TRACKING")]
    public ScoringManager scoringManager;

    void Start()
    {
        pointsPopupText.gameObject.SetActive(false);
    }

    public void SetAIDifficulty(int level)
    {
        aiDifficultyLevel = level;
        Debug.Log("Clothing Manager set to AI Difficulty: " + aiDifficultyLevel);

        switch (level)
        {
            case 1:
                aiOutfitChanger = wanaiOutfitChanger;
                break;
            case 2:
                aiOutfitChanger = tooaiOutfitChanger;
                break;
            case 3:
                aiOutfitChanger = treeaiOutfitChanger;
                break;
        }

        Debug.Log("AI Difficulty set! Reference ready to SLAY ✨");
    }

    public void SelectClothingItem(ClothingItemData selectedItem, string theme)
    {

        ////Update the player's score
        //totalWinterPoints += selectedItem.winterPoints;
        ////Show the popup points
        //ShowPointsPopup(selectedItem.winterPoints);

        //update the player's score (modifying for themes)
        //totalPoints += points;

        //int basePoints = (theme == "summer") ? selectedItem.summerPoints : selectedItem.winterPoints;

        //Get the bonus points from the scoring manager 
        int bonusPoints = scoringManager.GetBonusForSingleItem(selectedItem, theme);

        //show the popup points for the player
        ShowBonusPopup(bonusPoints);

        //Recalculate based on selected outfits
        int updatedScore = CalculateOutfitScore(theme);
        int aiUpdatedScore = CalculateAiOutfitScore(theme);
        UpdatePlayerScoreUI(updatedScore);
        UpdateAiScoreUI(aiUpdatedScore);
        
    }


    void ShowBonusPopup(int bonusPoints)
    {
        pointsPopupText.text = "+" + bonusPoints.ToString() + " Bonus pts";
        pointsPopupText.gameObject.SetActive(true);

        // Reset scale
        pointsPopupText.transform.localScale = Vector3.zero;

        // Scale-up bounce using LeanTween
        LeanTween.scale(pointsPopupText.gameObject, Vector3.one * 1.2f, 0.3f).setEaseOutBack()
            .setOnComplete(() => {
                // Slight shrink back to normal size
                LeanTween.scale(pointsPopupText.gameObject, Vector3.one, 0.2f).setEaseOutExpo();
            });

        // Optional: fade glow (if you have an outline component)
        StartCoroutine(GlowEffect(pointsPopupText));

        StartCoroutine(HideBonusPopup());
    }

    IEnumerator HideBonusPopup()
    {
        yield return new WaitForSeconds(1f);
        pointsPopupText.gameObject.SetActive(false);
    }

    IEnumerator GlowEffect(TextMeshProUGUI text)
    {
        float duration = 1f;
        float elapsed = 0f;

        Outline outline = text.GetComponent<Outline>();
        if (outline == null)
        {
            outline = text.gameObject.AddComponent<Outline>();
        }

        Color startColor = new Color(1f, 1f, 0.5f, 0f); // transparent yellow glow
        Color endColor = new Color(1f, 1f, 0.5f, 0.8f); // bright yellow

        while (elapsed < duration)
        {
            float t = Mathf.PingPong(elapsed * 2f, 1f); // pingpong effect
            outline.effectColor = Color.Lerp(startColor, endColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        outline.effectColor = startColor; // reset glow at the end
    }

    //Need a method that will calculate the points of the current clothing worn
    public int CalculateOutfitScore(string theme)
    {
        int score = 0;

        //Let's try and get ClothingItemData from each equipped piece
        if (playerOutfitChanger.currentShirt != null)
        {
            var data = playerOutfitChanger.currentShirt.GetComponent<ClothingItemHolder>();   
            if (data != null)
            {
                //score += data.clothingItemData.winterPoints;
                //modifying to handle two themes
                score += GetThemePoints(theme, data.clothingItemData);
            }
        }

        if (playerOutfitChanger.currentPants != null)
        {
            var data = playerOutfitChanger.currentPants.GetComponent<ClothingItemHolder>();
            if (data != null) 
            {
                //score += data.clothingItemData.winterPoints; 
                //modifying for two themes
                score += GetThemePoints(theme, data.clothingItemData);
            }
        }

        if (playerOutfitChanger.currentShoes != null)
        {
            var data = playerOutfitChanger.currentShoes.GetComponent<ClothingItemHolder>();
            if (data != null) 
            {
                //score += data.clothingItemData.winterPoints; 
                //modifying for themes
                score += GetThemePoints(theme, data.clothingItemData);
            }
        }

        return score;
    }

    public int CalculateAiOutfitScore(string theme)
    {
        
        //// Scoring logic that changes with AI level
        //switch (aiDifficultyLevel)
        //{
        //    case 1:
        //        aiScore = CalculateScoreBasic(theme); // ez peasy
        //        break;
        //    case 2:
        //        aiScore = CalculateScoreIntermediate(theme); // medium spice
        //        break;
        //    case 3:
        //        aiScore = CalculateScoreAdvanced(theme); // full drag
        //        break;
        //}

        //////Update the AI live score
        //aiLiveScoreText.text = "AI: " + aiScore.ToString() + " pts";

        //return aiScore;

        if (aiOutfitChanger != null)
        {
            //let the active level difficulty calculate it's own score
            aiScore = aiOutfitChanger.CalculateAiOutfitScoreWithBonus(theme);

            ////Update the AI live score
            aiLiveScoreText.text = "AI: " + aiScore.ToString() + " pts";

            return aiScore;
        }
        else
        {
            Debug.LogWarning("AI Outfit Changer is not set in ClothingManager!");
            return 0;
        }
    }

    //methods for the different AI LEVELS
    //private int CalculateScoreBasic(string theme)
    //{
    //    aiScore = 0;

    //    if (wanaiOutfitChanger.currentShirt != null)
    //    {
    //        var data = wanaiOutfitChanger.currentShirt.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modifying for two themes . another if statement that needs to be expanded
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }

    //    if (wanaiOutfitChanger.currentPants != null)
    //    {
    //        var data = wanaiOutfitChanger.currentPants.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modyifying, yeah
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }

    //    if (wanaiOutfitChanger.currentShoes != null)
    //    {
    //        var data = wanaiOutfitChanger.currentShoes.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modifyin yeah yeah
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }
    //    return aiScore;
    //}

    //private int CalculateScoreIntermediate(string theme)
    //{
    //    aiScore = 0;
    //    if (tooaiOutfitChanger.currentShirt != null)
    //    {
    //        var data = tooaiOutfitChanger.currentShirt.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modifying for two themes . another if statement that needs to be expanded
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }

    //    if (tooaiOutfitChanger.currentPants != null)
    //    {
    //        var data = tooaiOutfitChanger.currentPants.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modyifying, yeah
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }

    //    if (tooaiOutfitChanger.currentShoes != null)
    //    {
    //        var data = tooaiOutfitChanger.currentShoes.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modifyin yeah yeah
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }
    //    return aiScore;
    //}

    //private int CalculateScoreAdvanced(string theme) 
    //{
    //    aiScore = 0;

    //    if (treeaiOutfitChanger.currentShirt != null)
    //    {
    //        var data = treeaiOutfitChanger.currentShirt.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modifying for two themes . another if statement that needs to be expanded
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }

    //    if (treeaiOutfitChanger.currentPants != null)
    //    {
    //        var data = treeaiOutfitChanger.currentPants.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modyifying, yeah
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }

    //    if (treeaiOutfitChanger.currentShoes != null)
    //    {
    //        var data = treeaiOutfitChanger.currentShoes.GetComponent<ClothingItemHolder>();
    //        if (data != null)
    //        {
    //            //score += data.clothingItemData.winterPoints;
    //            //modifyin yeah yeah
    //            aiScore += GetThemePoints(theme, data.clothingItemData);
    //        }
    //    }
    //    return aiScore; 
    //}

    //Method that will handle the themes and their related points
    private int GetThemePoints(string theme, ClothingItemData data)
    {
        switch (theme)
        {
            case "summer":
                return data.summerPoints;
            case "winter":
                return data.winterPoints;
            case "casual date":
                return data.casualDatePoints;
            case "artsy street style":
                return data.artStreetStylePoints;
            case "tv show audition":
                return data.tvShowAuditionPoints;
            default:
                return 0;
        }
    }

    public void UpdateAiScoreUI(int score)
    {
        if (aiLiveScoreText != null)
        {
            aiLiveScoreText.text = "AI: " + score + " pts";
        }
    }

    public void UpdatePlayerScore(int points)
    {
        playerScore += points;

        //update the UI
        UpdatePlayerScoreUI(points);
    }

    void UpdatePlayerScoreUI(int score)
    {
        if (playerLiveScoreText != null)
        {
            playerLiveScoreText.text = "Player: " + score + " pts";
        }
    }

    public void ResetPlayerScore()
    {
        //reset the players score to 0 at the beninnning of each roundddd
        totalPoints = 0;
        playerScore = 0;

        //update the ui to show the reset score
        UpdatePlayerScoreUI(0);
    }

   public void ResetAiScore()
   {
        //reset the score to 0 at the beninning of each round
        aiScore = 0;
        UpdateAiScoreUI(0);
   }

  
    // Method to trigger animation of bonus score
    public void AnimateBonusScoreInClothingManager(int baseScore, int bonus, string label)
    {
        // Start the coroutine for animating the bonus score
        if (label == "Player")
        {
            StartCoroutine(AnimateBonusScore(playerFinalScoreText, baseScore, bonus, label));
        }
        else if (label == "AI")
        {
            StartCoroutine(AnimateBonusScore(aiFinalScoreText, baseScore, bonus, label));
        }
    }

    // Your existing AnimateBonusScore coroutine (unchanged)
    IEnumerator AnimateBonusScore(TextMeshProUGUI scoreText, int baseScore, int bonus, string label, float delay = 0.5f)
    {
        //yield return new WaitForSeconds(delay);

        //int finalScore = baseScore + bonus;

        //scoreText.text = label + " Score: " + baseScore + " pts"
        //    + "\nBonus: +" + bonus
        //    + "\nTotal: " + finalScore + " pts";

        //SKIIIP

        scoreText.text = $"{label} \nScore: {baseScore} pts";
        yield return new WaitForSeconds(delay);

        scoreText.text += $"\n<color=#90258C>+{bonus} \nStyle Bonus!</color>";
        yield return new WaitForSeconds(0.3f);

        int finalScore = baseScore + bonus;
        int current = baseScore;

        while (current < finalScore)
        {
            current++;
            scoreText.text = $"{label} \nScore: {current} pts\n<color=#90258C>+{bonus} \nStyle Bonus!</color>"
            + "\nTotal: " + finalScore + " pts"; ;
            scoreText.ForceMeshUpdate();
            yield return new WaitForSeconds(0.05f);
        }
    }
}

////METHOD THAT UPDATES THE FINAL SCORE UI AFTER THE ROUND ENDS:
//public void UpdateFinalScoreUI(int playerFinalScore, int aiFinalScore, int playerBonus, int aiBonus)
//{
//    // Update the final score text for player
//    playerLiveScoreText.text = $"Player Score: {playerFinalScore} pts\nBonus: +{playerBonus}\nTotal: {playerFinalScore} pts";

//    // Update the final score text for AI
//    aiLiveScoreText.text = $"AI Score: {aiFinalScore} pts\nBonus: +{aiBonus}\nTotal: {aiFinalScore} pts";
//}

