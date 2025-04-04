using UnityEngine;


public class ClothingItemHolder : MonoBehaviour
{
    //TO BE ATTACHED TO EACH AND EVERY ITEM
    //You need the ClothingItemHolder on each GameObject so we can ask "What clothing item data are you?" at any time. Especially at round end.
    //It’s like giving each outfit a nametag at the Winter Fashion Show

    public ClothingItemData clothingItemData;
}
