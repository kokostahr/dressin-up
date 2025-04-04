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

    [Header("UI SETTINGS")]
    //Text that will display the countdown
    public TextMeshProUGUI timerText;
    //Panel that will show the different scores at the end of the round
    public GameObject scorePanel;
    //Text that will display the final/total score
    public TextMeshProUGUI playerFinalScoreText;
    //public TextMeshProUGUI robotFinalScoreText;
    //Referencing the clothing manager to count the points
    public ClothingManager clothingManager;


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
        //Show the score panel
        scorePanel.SetActive (true);
        playerFinalScoreText.text = "Total Score: " + clothingManager.totalWinterPoints + " points";
    }
}
