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
    public int playerFinalScore = 0;

    //Winner Tracking
    public int playerWins = 0;
    public int aiWins = 0;

    //Resetting the AI's clothing choices every round
    public Ai_OutfitChanger aiOutfitChanger;
    public Player_OutfitChange playerOutfitChanger;

    //This is going to track the current theme
    private string currentTheme;

    //idk why im adding this
    public static RoundManager Instance;

    [Header("MUSIC SETTINGS")]
    public AudioSource calmAudioSource;//The slow music track
    public AudioSource fastAudioSource; //Sped up version of the music track
    //public AudioClip calmMusic; 
    //public AudioClip fastMusic; 
    private float roundTimer; //Timer to track the round's time
    private bool isFastMusicPlaying = false;

    [Header("FLASH SETTINGS")]
    public Image flashPanelImg;
    private bool flashingStarted = false;


    private void Awake()
    {
        Instance = this;
    }

    public string GetCurrentTheme()
    {
        return currentTheme;
    }

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
        roundTimer = 0f;
        isFastMusicPlaying = false;
        //Starting the calm music at the beginning of the round
        //audioSource.clip = calmMusic;
        //audioSource.loop = true;
        //audioSource.Play();
        fastAudioSource.Stop(); //Incase it was playing
        calmAudioSource.time = 0f;
        calmAudioSource.Play();

        //starting the ai's outfit choosing again
        aiOutfitChanger.StartCoroutine(aiOutfitChanger.ChooseRandomOutfitDelay());

        //need to hide the score panel at the beginning
        scorePanel.SetActive(false);
        StartCoroutine(RoundCountdown());

        //Randomly set the theme for the round
        SetRandomTheme();
    }

    //A couroutine that adds a flash panel to the scene
    IEnumerator FlashCountDownWarning()
    {
        for (int i = 3; i> 0; i--)
        {
            //turn the flash on
            flashPanelImg.color = new Color(1f, 1f, 1f, 0.6f); //semi visible white
            yield return new WaitForSeconds(0.15f);

            //turn the flash off
            flashPanelImg.color = new Color(1f, 1f, 1f, 0f); //gone again
            yield return new WaitForSeconds(0.85f); //does about 1s per flash
        }
    }

    //Need a function that will countdown the time left and display it on screen
    IEnumerator RoundCountdown()
    {
        while (timeLeft > 0 &&  roundActive)
        {
            timeLeft -= Time.deltaTime;
            roundTimer += Time.deltaTime; //track the time since the round started
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft) + "s";

            //Check if the round is nearing the last 10 seconds
            if (roundTimer >= (roundDuration - 10f) && !isFastMusicPlaying)
            {
                //switch to the faster music for the last 10 seconds
                //audioSource.Stop();
                //audioSource.clip = fastMusic;
                //audioSource.loop = true;
                //audioSource.time = 0f;
                //audioSource.Play();
                calmAudioSource.Stop();
                fastAudioSource.time = 0f;
                fastAudioSource.Play();
                isFastMusicPlaying = true;
                timerText.color = Color.red;
            }

            //last three seconds flash must come on
            if (Mathf.Ceil(timeLeft) == 3 && !flashingStarted)
            {
                StartCoroutine(FlashCountDownWarning());
                flashingStarted = true; 
            }


            yield return null;
        }

        EndRound();
    }
   
    //Need a function that will set the scores active at the end of the round and determine the winner
    void EndRound()
    {
        roundActive = false;
        timerText.text = "Time's Up!";
        timerText.color = Color.white;

        //need to calculate the proper points of the final items on screen when round ends
        int playerFinalScore = clothingManager.CalculateOutfitScore(currentTheme);

        //calculate the ai's final score from the aiOutfitChanger
        int aiFinalScore = clothingManager.CalculateAiOutfitScore(currentTheme);

        //Show the score panel that will display AI and Player scores
        themeText.gameObject.SetActive(false);
        scorePanel.SetActive (true);
        //dont need to show these things anymore

        //playerFinalScoreText.text = "Player Score: " + playerFinalScore + " points";
        //aiFinalScoreText.text = "AI Score: " + aiFinalScore + " points";

        //audiostuffff
        calmAudioSource.Play();
        fastAudioSource.Stop();
        isFastMusicPlaying = false;

        //Need to hide all the clothes again
        aiOutfitChanger.AIHideAllRoundReset();
        playerOutfitChanger.PlayerHideAllRoundReset();

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
            //increment if player wins
            playerWins++;
        }
        else if (playerFinalScore < aiFinalScore)
        {
            roundResultText.gameObject.SetActive(true);
            roundResultText.text = "Damn, the AI won this round <<!";
            //increment if ai wins
            aiWins++;
        }
        else
        {
            roundResultText.gameObject.SetActive(true);
            roundResultText.text = "Oh! It's a tie... -_-??";
        }
    }

    void SetRandomTheme()
    {
        //Let's set the theme randomly to summer or WINTER. this is gonna be hard LOL
        currentTheme = Random.Range(0, 2) == 0 ? "summer" : "winter";
        themeText.text = "Theme: " + currentTheme.ToUpper(); //The hell this mean?
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
        //turn off the fast music and reset the round timer
        isFastMusicPlaying = false;
        roundTimer = 0f;
        //turn off the flash
        flashingStarted = false;

        //Reset AI Score for next round
        clothingManager.ResetAiScore();
        //Reset Player Score for next rround
        clothingManager.ResetPlayerScore();

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

        //Reset the music to calm for the next round
        //audioSource.Stop(); //stop the fast music or anything else
        //audioSource.clip = calmMusic;
        //audioSource.loop = true;
        //audioSource.time = 0f; //reset the music position to start
        //audioSource.Play();
        fastAudioSource.Stop(); //Incase it was playing
        calmAudioSource.time = 0f;
        calmAudioSource.Play();
        

        //call the AI outfitchanger coroutine to reset its clothes at the start of each round
        StartCoroutine(aiOutfitChanger.ChooseRandomOutfitDelay());

        //Reset the theme
        SetRandomTheme();

        //Resetting the round countdown
        StartCoroutine (RoundCountdown());


    }

    //method to end the game
    void EndGame()
    {
        timerText.text = "Game Over \U0001F910"; //please don't be surprised future me. These are emojis in 'unicode' form
        scorePanel.SetActive(true);

        //displaying the total wins
        if (playerWins > aiWins)
        {
            roundResultText.text = "Congratulations! You've outstyled the AI!";
            //aiFinalScoreText.text = "-_-??";
            //playerFinalScoreText.text = "You got the most wins! Close the game. Its finshed.";
        }
        else if (playerWins < aiWins)
        {
            roundResultText.text = "Sadly you lost. The Ai outstyled you!";
            //aiFinalScoreText.text = "The robot got the most wins. Close the game, It's finished.";
            //playerFinalScoreText.text = "The robot beat you.";
        }
        
        themeText.gameObject.SetActive (false);
        themePopupText.gameObject .SetActive (false);
    }


}
