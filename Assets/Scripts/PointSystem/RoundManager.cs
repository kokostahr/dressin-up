using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [Header("ROUND SETTINGS")]
    //the length of each round. 
    public float roundDuration = 10f;
    private float timeLeft;
    private bool roundActive = false;
    //variables that enable there to be multiple rounds. 
    public int totalRounds = 3;
    private int currentRound = 1;

    [Header("UI SETTINGS")]
    //Text that will display the countdown
    public TextMeshProUGUI timerText;
    //Panel that will show the different scores at the end of the round
    public GameObject scorePanel;
    //Text that will display the final/total score
    public TextMeshProUGUI playerFinalScoreText;
    public TextMeshProUGUI aiFinalScoreText;
    //Referencing the clothing manager to count the points
    public ClothingManager clothingManager;
    //text that will display what the next theme is
    public TextMeshProUGUI themeText;
    public TextMeshProUGUI themePopupText;
    //calling the player outfit change just to make the panels inactive
    public Player_OutfitChange player_OutfitChange;
    //Need text showing who won the round
    public TextMeshProUGUI roundResultText;

    //AI related logic for when I make an AI manager...just a place holder for now
    public int aiFinalScore = 0;

    
    void Start()
    {
        //When the game starts the first round should begin as well..duh
        StartRound();
    }


    //Function that starts each round..very self explanatory
    public void StartRound()
    {
        timeLeft = roundDuration;
        roundActive = true;
        //need to hide the score panel at the beginning
        scorePanel.SetActive(false);
        StartCoroutine(RoundCountdown());
    }

    //Need a function that will countdown the time left and display it on screen
    IEnumerator RoundCountdown()
    {
        while (timeLeft > 0 &&  roundActive)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft) + "s";
            yield return null;
        }

        EndRound();
    }
   
    //Need a function that will set the scores active at the end of the round and determine the winner
    void EndRound()
    {
        roundActive = false;
        timerText.text = "Time's Up!";

        //need to calculate the proper points of the final items on screen when round ends
        int playerFinalScore = clothingManager.CalculateOutfitScore();

        //Show the score panel that will display AI and Player scores
        scorePanel.SetActive (true);
        themeText.gameObject.SetActive (false);
        playerFinalScoreText.text = "Player Score: " + playerFinalScore + " points";
        aiFinalScoreText.text = "AI Score: " + aiFinalScore + " points";

        //Then compare scores to determine the winner of the round.
        CompareScores(playerFinalScore, aiFinalScore);

        //Wait the start next round or end game
        StartCoroutine(WaitAndStartNextRound());
    }

    void CompareScores(int playerFinalScore, int aiFinalScore)
    {
        if (playerFinalScore > aiFinalScore)
        {
            roundResultText.gameObject.SetActive (true);
            roundResultText.text = "Congrats, you won this round O.o!?";
        }
        else if (playerFinalScore < aiFinalScore)
        {
            roundResultText.gameObject.SetActive(true);
            roundResultText.text = "Damn, the AI won this round <<!";
        }
        else
        {
            roundResultText.gameObject.SetActive(true);
            roundResultText.text = "Oh! It's a tie... -_-??";
        }
    }

    public void StartNextRound()
    {
        if (currentRound < totalRounds)
        {
            currentRound++;
            ResetRound();

        }
        else
        {
            EndGame(); 
        }
    }

    //co-routine that will wait for a while after the round has ended to show the score and give the next theme
    IEnumerator WaitAndStartNextRound()
    {
        yield return new WaitForSeconds(4f);
        themePopupText.gameObject.SetActive(true);
        StartNextRound();
    }

    void ResetRound()
    {
        scorePanel.SetActive (false); //Hiding the scorepanel again
        //hide the clothing panels
        player_OutfitChange.shirtPanel.SetActive (false);
        player_OutfitChange.pantsPanel.SetActive (false);
        player_OutfitChange.shoePanel.SetActive (false);
        //hide the theme popup again
        themePopupText.gameObject.SetActive(false);
        //bring the theme text back
        themeText.gameObject.SetActive(true);
        //turn off the round result text
        roundResultText.gameObject.SetActive(false);
        //Reset the timer and clear 
        roundActive = true;
        timeLeft = roundDuration;
        StartCoroutine (RoundCountdown());
    }

    //method to end the game
    void EndGame()
    {
        timerText.text = "Game Over \U0001F910"; //please don't be surprised future me. These are emojis in 'unicode' form
        scorePanel.SetActive(true);
        playerFinalScoreText.text = "Close the game. It's finished \U0001F928";
        themeText.gameObject.SetActive (false);
        themePopupText.gameObject .SetActive (false);
    }

}
