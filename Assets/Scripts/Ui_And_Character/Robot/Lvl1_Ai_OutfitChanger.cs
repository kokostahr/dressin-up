using System.Collections;
using UnityEngine;

public class Lvl1_Ai_OutfitChanger : Ai_OutfitChanger
{
    public override IEnumerator ChooseRandomOutfitDelay()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PickWithDeliberation(aishirts, (chosen) =>
        {
            currentShirt = chosen;
            UpdateAIShirtDisplay(currentShirt);
            if (clothingManager != null)
            {
                clothingManager.UpdateAiScoreUI(clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme()));
            }
        }));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PickWithDeliberation(aipants, (chosen) =>
        {
            currentPants = chosen;
            UpdateAIPantsDisplay(currentPants);
            ShowRandomComment();
            if (clothingManager != null)
            {
                clothingManager.UpdateAiScoreUI(clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme()));
            }
        }));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PickWithDeliberation(aishoes, (chosen) =>
        {
            currentShoes = chosen;
            UpdateAIShoesDisplay(currentShoes);
            if (clothingManager != null)
            {
                clothingManager.UpdateAiScoreUI(clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme()));
            }
           
        }));
    }

    public override int CalculateAiOutfitScoreWithBonus(string theme)
    {
        //Calculate the base score for this specific AI level
        int baseScore = clothingManager.CalculateAiOutfitScore(theme);

        //then access the equipped items directly in this class
        ClothingItemData[] equippedItems = new ClothingItemData[3];
        if (currentShirt != null)
        {
            equippedItems[0] = currentShirt?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        }
        if (currentPants != null)
        {
            equippedItems[1] = currentPants?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        }
        if (currentShoes != null)
        {
            equippedItems[2] = currentShoes?.GetComponent<ClothingItemHolder>()?.clothingItemData;
        }

        //calculate base score based on equipped items and theme
        if (clothingManager != null)
        {
            if (equippedItems[0] != null) baseScore += clothingManager.GetThemePoints(theme, equippedItems[0]);
            if (equippedItems[1] != null) baseScore += clothingManager.GetThemePoints(theme, equippedItems[1]);
            if (equippedItems[2] != null) baseScore += clothingManager.GetThemePoints(theme, equippedItems[2]);
        }

        int bonus = 0;
        if (scoringManager != null)
        {
            //use scoring manager to calculate the bonus points
            bonus = scoringManager.AiBonusPoints(equippedItems, theme);
        }

        //then the total score for the AI level
        int totalScore = baseScore + bonus;

        // Optionally, update the ScoringManager's aiTotalScore here if needed elsewhere
        if (scoringManager != null)
        {
            scoringManager.aiTotalScore = totalScore;
        }

        return totalScore;
    }

    void ShowRandomComment()
    {
        foreach (GameObject comment in aiOutfitComments)
            comment.SetActive(false);

        int index = Random.Range(0, aiOutfitComments.Length);
        aiOutfitComments[index].SetActive(true);
        StartCoroutine(HideCommentAfterDelay(aiOutfitComments[index], 3.5f));
    }

    public override void SetPreferredTag(string tag)
    {
        string[] possibleTags = { "flirty", "cozy", "chic", "bold", "warm", "edgy", "vibrant" };
        currentPreferredTags.Clear();

        if (!string.IsNullOrEmpty(tag))
            currentPreferredTags.Add(tag);

        // Add 2 more random unique tags (for a total of 3)
        while (currentPreferredTags.Count < 2)
        {
            string randomTag = possibleTags[Random.Range(0, possibleTags.Length)];
            if (!currentPreferredTags.Contains(randomTag))
            {
                currentPreferredTags.Add(randomTag);
            }
        }

        StartCoroutine(ShowAiStyleMood());
        Debug.Log("Lvl2 AI slays with: " + string.Join(", ", currentPreferredTags));
    }

    protected override IEnumerator ShowAiStyleMood()
    {
        string mood = string.Join(" & ", currentPreferredTags);
        aiStyleMoodText.text = $"L2 AI is serving: <b>{mood.ToUpper()}</b> 💃";
        yield return new WaitForSeconds(1.2f);
    }
}
