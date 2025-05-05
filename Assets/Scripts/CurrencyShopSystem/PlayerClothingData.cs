using UnityEngine;

public static class PlayerClothingData 
{
    
    public static bool IsItemBought(string itemName)
    {
        return PlayerPrefs.GetInt("Bought_" + itemName, 0 ) == 1;
    }

    public static void MarkItemAsBought(string itemName)
    {
        PlayerPrefs.SetInt("Bought_" + itemName, 1);
        PlayerPrefs.Save();
    }

    public static void ResetAllPurchasesForDebug()
    {
        PlayerPrefs.DeleteAll();//delete this once game is finished PLEASE
    }

}
