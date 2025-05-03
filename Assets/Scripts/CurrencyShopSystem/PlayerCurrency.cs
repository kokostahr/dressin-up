using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DentedPixel;

public class PlayerCurrency : MonoBehaviour   
{
    //Making Reference to the player points systems and the round manager system so that we can see how many points the player scored
    //But also add that to their currency. 
    [Header("POINTS REFERENCE")]

    public ClothingManager clothingManager;
    public RoundManager roundManager;


    [Header("Points SetUp")]

    public static PlayerCurrency Instance;
    public int fashionPoints = 0;
    public TextMeshProUGUI fashionPointsText;
    public GameObject declinedMessage;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    //A function that will add the player's points at the end of each round.
    public void AddPoints(int amount)
    {
        //int amount = roundManager.playerFinalScore;
        //Trying my luck to reference the already existing points and get that to show at the end of each round
        fashionPoints += amount;
        // Updating the text whenever the player gets more points
        fashionPointsText.text = "Fashion Pts: " + amount.ToString();
    }

    //A function that will check if the player has enough points to spend, then if they do it will reduce their points.
    //if not, it will let them know that "HEY, YOU'RE TOO BROKE BABES"
    public bool SpendPoints(int amount)
    {
        //if player has more points then they wanna spend, then minus the spending from their accumulated fashion points
        if (fashionPoints >= amount)
        {
            fashionPoints -= amount;
            Debug.Log("Player has spent for " + amount + " points");
            //will do the UI UPDATE HERE ALSO
            //fashionPointsText.Text = playerScore.ToString()
            return true;
        }
        else //not enuf money? don't let them buy 
        {
            Debug.Log("PLayer doesn't have enuf points");
            //UI update to let player know they cant buy. Set the declined message true, add LEANTWEEN for juicy animatioms
            declinedMessage.SetActive(true);
            //Need a co-routine that will make the declinded message disappear after a second or smth
            //Space for the co-routine>>
            return false;
        }
    }
    

}
