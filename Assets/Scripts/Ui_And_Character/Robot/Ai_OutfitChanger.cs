using NUnit.Framework;
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
        //HideAll(aishirts);
        //HideAll(aipants);
        //HideAll(aishoes);
        ChooseRandomOutfit();
    }

    // Function to hide all items in a category
    void HideAll(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
    }
    
    //Function that will help the AI randomly pick clothes
    public void ChooseRandomOutfit()
    {
        currentShirt = PickRandom(aishirts);
        currentPants = PickRandom(aipants);
        currentShoes = PickRandom(aishoes);
    }

    GameObject PickRandom(GameObject[] options)
    {
        if (options == null || options.Length == 0)
        {
            Debug.LogWarning("AI Outfitchanger: One of the arrays is empty");
            return null;
        }

        //Hide all the items on the AI character first
        foreach (var item in options)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }

        //Pick items randomly
        int randomIndex = Random.Range(0, options.Length);
        GameObject chosen = options[randomIndex];
        if (chosen != null)
        {
            chosen.SetActive(true);
        }
        return chosen;
    }

}
