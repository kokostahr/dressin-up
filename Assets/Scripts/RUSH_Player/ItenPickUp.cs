using UnityEngine;


public class ItenPickUp : MonoBehaviour
{
    public ClothingManager clothingManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var itemHolder = other.GetComponent<ClothingItemHolder>();

        if (itemHolder != null)
        {
            // Add to list
            clothingManager.playerCollectedItems.Add(itemHolder.clothingItemData);

            // Get current theme from RoundManager
            string currentTheme = RoundManager.Instance.GetCurrentTheme();

            // Tell clothing manager to score this item
            clothingManager.SelectClothingItem(itemHolder.clothingItemData, currentTheme);

            // Destroy or hide the collected item
            Destroy(other.gameObject);
        }
    }
}
