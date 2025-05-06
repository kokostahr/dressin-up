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
            clothingManager.UpdateAiScoreUI(clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme()));
        }));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PickWithDeliberation(aipants, (chosen) =>
        {
            currentPants = chosen;
            UpdateAIPantsDisplay(currentPants);
            ShowRandomComment();
            clothingManager.UpdateAiScoreUI(clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme()));
        }));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PickWithDeliberation(aishoes, (chosen) =>
        {
            currentShoes = chosen;
            UpdateAIShoesDisplay(currentShoes);
            clothingManager.UpdateAiScoreUI(clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme()));
        }));
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
