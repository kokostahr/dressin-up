using NUnit.Framework;
using System.Collections;
using UnityEngine;

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
        }));

        yield return new WaitForSeconds(6f);//Let AI PAUSE AGAIN

        yield return StartCoroutine(PickWithDeliberation(aipants, (chosen) => {
            currentPants = chosen;
        }));

        yield return new WaitForSeconds(10f);//LET IT PAUSE ONCE MORE

        yield return StartCoroutine(PickWithDeliberation(aishoes, (chosen) => {
            currentShoes = chosen;
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


