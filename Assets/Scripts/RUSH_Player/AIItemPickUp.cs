using UnityEngine;

public class AIItemPickUp : MonoBehaviour
{
    public ClothingManager clothingManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var itemHolder = other.GetComponent<ClothingItemHolder>();

        if (itemHolder != null)
        {
            // Get current theme from RoundManager
            string currentTheme = RoundManager.Instance.GetCurrentTheme();

            // Tell clothing manager to score this item
            clothingManager.SelectClothingItemForAI(itemHolder.clothingItemData, currentTheme);

            // Destroy or disable the object
            Destroy(other.gameObject);
        }
    }
}
