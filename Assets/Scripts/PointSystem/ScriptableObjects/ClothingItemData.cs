using UnityEngine;


[CreateAssetMenu(fileName = "NewClothingItem", menuName ="Clothing System/ClothingItem")]
public class ClothingItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon; //For the UI display
    public int winterPoints; //For the relevant points associated with Winter theme
    public int summerPoints; //for the relevant points associated with Summer theme
    public int casualPoints; //for the relevant points associated with a Casual theme
    public int cozyPoints; //For the relevant points associated with a cozy theme
    public string[] itemTag; //Made it an array so I can add more than one tag.



}
