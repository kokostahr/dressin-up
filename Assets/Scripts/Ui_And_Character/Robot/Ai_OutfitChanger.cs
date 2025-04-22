using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Ai_OutfitChanger : MonoBehaviour
{
    //Arrays to hold all the relevant Items. Yes
    [Header("ARRAY CATEGORY")]
    public GameObject[] aishirts;
    public GameObject[] aipants;
    public GameObject[] aishoes;

    //Variables to track the current worn item
    [HideInInspector] public GameObject currentShirt;
    [HideInInspector] public GameObject currentPants;
    [HideInInspector] public GameObject currentShoes;

    [Header("AI DISPLAY IMAGES")]
    public Image aiShirtDisplay, aiPantsDisplay, aiShoeDisplay;

    [Header("AI UI SPRITE ARRAY")]
    public Sprite[] aiShirtSprites, aiPantsSprites, aiShoeSprites;

    [Header("REFERENCES")]
    public ClothingManager clothingManager;

    [Header("TALK TALK LINES")]
    public GameObject[] aiOutfitComments;

    [Header("AI PREFERENCE SELECTION")]
    [HideInInspector] public List<string> currentPreferredTags = new List<string>();
    


    void Start()
    {
        HideAll(aishirts);
        HideAll(aipants);
        HideAll(aishoes);
        //first hide all active comments
        foreach (GameObject comment in aiOutfitComments)
        {
            comment.SetActive(false);
        }
        //StartCoroutine(ChooseRandomOutfitDelay());
    }

    // Function to hide all items in a category
    void HideAll(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
    }

    public void AIHideAllRoundReset()
    {
        HideAll(aishirts);
        HideAll(aipants);
        HideAll(aishoes);
    }

    //Co-routine that will make the AI look like its still thinking about its choices xD
    public IEnumerator ChooseRandomOutfitDelay()
    {
        //Wait a bit before choosing shirt
        yield return new WaitForSeconds(2f); //Let AI pause before it starts selecting
        yield return StartCoroutine(PickWithDeliberation(aishirts, (chosen) =>
        {
            currentShirt = chosen;
            //update the UI to show the selected shirt
            UpdateAIShirtDisplay(currentShirt);
            int aiScore = clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme());
            clothingManager.UpdateAiScoreUI(aiScore); //updating the live score?
        }));


        yield return new WaitForSeconds(6f);//Let AI PAUSE AGAIN

        yield return StartCoroutine(PickWithDeliberation(aipants, (chosen) => {
            currentPants = chosen;
            //update the UI to show selected pants
            UpdateAIPantsDisplay(currentPants);
            //A simple way to make the AI comment when it makes a choice
            //first hide all active comments
            foreach (GameObject comment in aiOutfitComments)
            {
                comment.SetActive(false);
            }

            //Pick a new one to show
            int randomIndex = Random.Range(0, aiOutfitComments.Length);
            aiOutfitComments[randomIndex].SetActive(true);

            //ANOTHER co routine to hide the comment after a short amount of time
            StartCoroutine(HideCommentAfterDelay(aiOutfitComments[randomIndex], 4f));
            
            int aiScore = clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme());
            clothingManager.UpdateAiScoreUI(aiScore);//updating the live score?
        }));

        yield return new WaitForSeconds(5f);//LET IT PAUSE ONCE MORE
       
        yield return StartCoroutine(PickWithDeliberation(aishoes, (chosen) => {
            currentShoes = chosen;
            //update the UI to show selected shoes
            UpdateAIShoesDisplay(currentShoes);

            int aiScore = clothingManager.CalculateAiOutfitScore(RoundManager.Instance.GetCurrentTheme());
            clothingManager.UpdateAiScoreUI(aiScore); //updating the live score?
        }));

       // FinaliseOutfitAndScore(RoundManager.Instance.GetCurrentTheme());
    }

    //Coroutine that will make the AI pick multiple options before settling on one
    IEnumerator PickWithDeliberation(GameObject[] options, System.Action<GameObject> callback)
    {
        if (options == null || options.Length == 0)
        {
            Debug.LogWarning("AI Outfitchanger: One of the arrays is empty");
            callback(null);
            yield break;
        }

        //Create a weighted list of items that match the tags
        List<GameObject> preferredItems = new List<GameObject>();
        foreach (GameObject item in options)
        {
            if (item == null)
            {
                Debug.LogWarning("Item is null in AI selection pool.");
                continue;
            }

            ClothingItemData data = item.GetComponent<ClothingItemData>();
            if (data != null && data.itemTag.Any(tag => currentPreferredTags.Contains(tag)))
            {
                preferredItems.Add(item);
            }
            else if (data == null)
            {
                Debug.LogWarning("Missing ClothingItemData on: " + item.name);
            }
        }

        GameObject[] pool = preferredItems.Count > 0 ? preferredItems.ToArray() : options; //to fallback if there are no matches


        //Ensure all stay hidden
        foreach (var item in options)
        {
            item.SetActive(false);
        }

        int firstIndex = Random.Range(0, pool.Length);
        pool[firstIndex].SetActive(true);
        yield return new WaitForSeconds(1f);
        pool[firstIndex].SetActive(false);

        int secondIndex = Random.Range(0, pool.Length);
        while (secondIndex == firstIndex)
            secondIndex = Random.Range(0, pool.Length);

        pool[secondIndex].SetActive(true);
        yield return new WaitForSeconds(1f);
        pool[secondIndex].SetActive(false);

        int finalIndex = Random.Range(0, pool.Length);
        pool[finalIndex].SetActive(true);

        //send the chosen item back through callback

        callback(pool[finalIndex]);
    }

    //Method to hide the comment after a short amount of time
    IEnumerator HideCommentAfterDelay(GameObject commentObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        commentObj.SetActive(false);
    }

    void UpdateAIShirtDisplay(GameObject chosenShirt)
    {
        //find the index of the chosen shirt
        int index = System.Array.IndexOf(aishirts, chosenShirt);
        if (index >= 0 && index < aiShirtSprites.Length)
        {
            aiShirtDisplay.sprite = aiShirtSprites[index];
        }
    }

    void UpdateAIPantsDisplay(GameObject chosenPants)
    {
        //find the index of the chosen pants
        int index = System.Array.IndexOf(aipants, chosenPants);
        if (index >= 0 && index < aiPantsSprites.Length)
        {
            aiPantsDisplay.sprite = aiPantsSprites[index];
        }
    }

    void UpdateAIShoesDisplay(GameObject chosenShoes)
    {
        //Find the index of the selected shoes
        int index = System.Array.IndexOf(aishoes, chosenShoes);
        if (index >= 0 && index < aiShoeSprites.Length)
        {
            aiShoeDisplay.sprite = aiShoeSprites[index];
        }
    }

    public void FinaliseOutfitAndScore(string theme)
    {
        if (clothingManager != null)
        {
            clothingManager.CalculateAiOutfitScore(theme);
        }

    }

    //Function that will randomly assign two tags to the AI at the start of each round
    public void SetPreferredTag( string tag )
    {
        string[] possibleTags = { "flirty", "cozy", "chic", "bold", "warm", "edgy", "vibrant" };
        currentPreferredTags.Clear();

        currentPreferredTags.Add(tag); //USE THE PASSED IN TAG

        //then add a second different tag randomly
        while(currentPreferredTags.Count < 2)
        {
            string randomTag = possibleTags[Random.Range(0, possibleTags.Length)];
            if (!currentPreferredTags.Contains(randomTag))
            {
                currentPreferredTags.Add(randomTag);
            }
        }

        Debug.Log("Lil Ai prefers: " + currentPreferredTags[0] + " and " + currentPreferredTags[1]);
    }

}
    
    ////Function that will help the AI randomly pick clothes. Made it a coroutine
    //public void ChooseRandomOutfit()
    //{
    //    currentShirt = PickRandom(aishirts);
    //    currentPants = PickRandom(aipants);
    //    currentShoes = PickRandom(aishoes);
    //}

    //GameObject PickRandom(GameObject[] options)
    //{
    //    if (options == null || options.Length == 0)
    //    {
    //        Debug.LogWarning("AI Outfitchanger: One of the arrays is empty");
    //        return null;
    //    }

    //    //Hide all the items on the AI character first
    //    foreach (var item in options)
    //    {
    //        if (item != null)
    //        {
    //            item.SetActive(false);
    //        }
    //    }

    //    //Pick items randomly
    //    int randomIndex = Random.Range(0, options.Length);
    //    GameObject chosen = options[randomIndex];
    //    if (chosen != null)
    //    {
    //        chosen.SetActive(true);
    //    }
    //    return chosen;
    //}


