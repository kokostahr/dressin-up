using NUnit.Framework;
using System.Collections;
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

    [Header("TALK TALK LINES")]
    public TextMeshProUGUI aiTalkTalkText;
    string[] outfitChoiceComments =
    {
        "This colour suits me so well",
        "I just know I'm gonna win this round",
        "There's no theme that I cannot conquer",
        "I'm stylish and practial...",
        "My calculations predict that I will win",
        "Don't I look impressive?",
        "I haven't finished yet, and it's already eating",
        "I'm a stylish icon, just look at me!",
        "LOOK AT THIS OUTFIT!? End the game, we know who the winner is",
        "I feel and look so pretty, wow..",
        "Just the shoes, then I'm gonna slay this round",
        "Wow, why is it so hard to make a choice?!",
        "Okay, I'm feeling very unsure of my choices...",
        "I do NOT look like I'm going to win.",
        "Damn, why did I choose this?!"
    };

    void Start()
    {
        HideAll(aishirts);
        HideAll(aipants);
        HideAll(aishoes);
        StartCoroutine(ChooseRandomOutfitDelay());
    }

    // Function to hide all items in a category
    void HideAll(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
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
            
        }));

        yield return new WaitForSeconds(6f);//Let AI PAUSE AGAIN

        yield return StartCoroutine(PickWithDeliberation(aipants, (chosen) => {
            currentPants = chosen;
            //update the UI to show selected pants
            UpdateAIPantsDisplay(currentPants);
            //A simple way to make the AI comment when it makes a choice
            int outfitIndex = Random.Range(0, outfitChoiceComments.Length);
        }));

        yield return new WaitForSeconds(7f);//LET IT PAUSE ONCE MORE

        yield return StartCoroutine(PickWithDeliberation(aishoes, (chosen) => {
            currentShoes = chosen;
            //update the UI to show selected shoes
            UpdateAIShoesDisplay(currentShoes);
        }));
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

        //Ensure all stay hidden
        foreach (var item in options)
        {
            item.SetActive(false);
        }

        int firstIndex = Random.Range(0, options.Length);
        options[firstIndex].SetActive(true);
        yield return new WaitForSeconds(1f);
        options[firstIndex].SetActive(false);

        int secondIndex = Random.Range(0, options.Length);
        while (secondIndex == firstIndex)
            secondIndex = Random.Range(0, options.Length);

        options[secondIndex].SetActive(true);
        yield return new WaitForSeconds(1f);
        options[secondIndex].SetActive(false);

        int finalIndex = Random.Range(0, options.Length);
        options[finalIndex].SetActive(true);

        //send the chosen item back through callback

        callback(options[finalIndex]);
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


