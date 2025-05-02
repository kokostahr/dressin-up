using UnityEngine;


[CreateAssetMenu(fileName = "NewClothingItem", menuName ="Clothing System/ClothingItem")]
public class ClothingItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon; //For the UI display
    public int winterPoints; //For the relevant points associated with Winter theme
    public int summerPoints; //for the relevant points associated with Summer theme
    public int casualDatePoints; //for the relevant points associated with a Casual Date theme
    public int artStreetStylePoints; //For the relevant points associated with a Artsy Street Style theme
    public int tvShowAuditionPoints; //For the relevant points associated with a TV Show Audition theme.
    public string[] itemTag; //Made it an array so I can add more than one tag.
    //new stuff related to player purchase
    public int cost; //Self explanatory. Cost of the item
    public bool isBought; //A bool to check if the player has purchased the item. if not (false) then the item
                        //Is only avaliable in store. If yes (true) then the item goes to their wardrobe. 
}
