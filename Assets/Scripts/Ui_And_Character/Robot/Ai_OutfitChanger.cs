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
        currentShirt = SetRandomItem(aishirts);
        currentPants = SetRandomItem(aipants);
        currentShoes = SetRandomItem(aishoes);
    }

    GameObject SetRandomItem(GameObject[] items)
    {
        //Hide all the items on the AI character
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }


        //Pick items randomly
        int randomIndex = Random.Range(0, items.Length);
        GameObject selectedItem = items[randomIndex];
        selectedItem.SetActive(true);
        return selectedItem;
    }

}
