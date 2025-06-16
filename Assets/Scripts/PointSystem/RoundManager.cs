using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

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

    //text that will display what the next theme is
    public TextMeshProUGUI themeText;
    public TextMeshProUGUI themePopupText;
    //calling the player outfit change just to make the panels inactive
    public Player_OutfitChange player_OutfitChange;
    //Need text showing who won the round
    public TextMeshProUGUI roundResultText;
    public GameObject nextRoundButton;

    [Header("POINT COMPARISON SETTINGS")]
    //Referencing the clothing manager to count the points
    public ClothingManager clothingManager;
    public ScoringManager scoringManager;

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

    [Header("THE AI PREFERENCES")]
    private string currentAiPreferredTag;


    [Header("SCENE SETTINGS")]
    public GameObject sceneUI;

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

    [Header("POINT CURRENCY SETTINGS")]
    public PlayerCurrency playerCurrency;
    public TextMeshProUGUI playerRoundWinnerText;
    public TextMeshProUGUI aiRoundWinnerText;
    public int roundWinner = 15;


    private void Awake()
    {
        Instance = this;
        sceneUI.SetActive(false);
    }

    public string GetCurrentTheme()
    {
        return currentTheme;
    }

    void Start()
    {
        //When the game starts the first round should begin as well..duh
        StartRound();

        //deactivate the final score text ui at the start of the game
        clothingManager.playerFinalScoreText.gameObject.SetActive(false);
        clothingManager.aiFinalScoreText.gameObject.SetActive(false);

        //deactivate the winner bonus text at the beginning of the game
        playerRoundWinnerText.gameObject.SetActive(false);
        aiRoundWinnerText.gameObject.SetActive(false);
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

        //randomly set the AI's tag preferences for the round
        SetRandomAiPreference();
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
        //set the live score text from the clothing manager inactive
        clothingManager.playerLiveScoreText.gameObject.SetActive(false);
        clothingManager.aiLiveScoreText.gameObject.SetActive(false);

        //need to calculate the proper points of the final items on screen when round ends
        ScoringManager.Instance.UpdatePlayerScore(currentTheme);
        ScoringManager.Instance.UpdateAIScore(currentTheme);

        int playerBaseScore = clothingManager.CalculateOutfitScore(currentTheme);
        int aiBaseScore = clothingManager.CalculateAiOutfitScore(currentTheme);

        //get the bonus points 
        int playerBonus = ScoringManager.Instance.lastPlayerBonus;
        int aiBonus = ScoringManager.Instance.lastAIBonus;

        //calculate the FINAL SCORE FOR BOTH
        playerFinalScore = playerBaseScore + playerBonus;
        aiFinalScore = aiBaseScore + aiBonus;

        //THEN ADD THE POINTS TO THE PLAYER'S CURRNCY
        playerCurrency.AddPoints(playerFinalScore);


        // Activate final score UI now that we're showing results
        clothingManager.playerFinalScoreText.gameObject.SetActive(true);
        clothingManager.aiFinalScoreText.gameObject.SetActive(true);

        // ✨ Start coroutine that handles animations, THEN compares scores, THEN next round
        StartCoroutine(HandleEndRoundSequence(playerBaseScore, aiBaseScore, playerBonus, aiBonus));



        ////Update the finalscores in the UI USING THE CLOTHING MANAGER
        //clothingManager.UpdateFinalScoreUI(playerFinalScore, aiFinalScore, playerBonus, aiBonus);

        //Show the score panel that will display AI and Player scores
        themeText.gameObject.SetActive(false);
        scorePanel.SetActive (true);


        //dont need to show these things anymore
        //playerFinalScoreText.text = playerFinalScore.ToString();
        //aiFinalScoreText.text = aiFinalScore.ToString();

        //audiostuffff
        calmAudioSource.Play();
        fastAudioSource.Stop();
        isFastMusicPlaying = false;

        //Need to hide all the clothes again
        aiOutfitChanger.AIHideAllRoundReset();
        playerOutfitChanger.PlayerHideAllRoundReset();

       
        ////Then compare scores to determine the winner of the round.
        //CompareScores(playerFinalScore, aiFinalScore);

        ////Wait the start next round or end game
        //StartCoroutine(WaitAndStartNextRound());

    }


    //coroutine that will... handle all the co-routines in the end round method
    IEnumerator HandleEndRoundSequence(int playerBaseScore, int aiBaseScore, int playerBonus, int aiBonus)
    {
        clothingManager.AnimateBonusScoreInClothingManager(playerBaseScore, playerBonus, "Player");
        clothingManager.AnimateBonusScoreInClothingManager(aiBaseScore, aiBonus, "AI");

        // Only once both animations are done
        CompareScores(playerFinalScore, aiFinalScore);
        yield return new WaitForSeconds(6f);
        //themePopupText.gameObject.SetActive(true);
        //StartNextRound();
    }


    void CompareScores(int playerFinalScore, int aiFinalScore)
    {
        
        if (playerFinalScore > aiFinalScore)
        {
            roundResultText.gameObject.SetActive (true);
            roundResultText.text = "Congrats. You won this round.";
            //increment if player wins
            playerWins++;

            //Then add 15 points to their fashion points
            StartCoroutine(PlayerRoundWinnerText());

        }
        else if (playerFinalScore < aiFinalScore)
        {
            roundResultText.gameObject.SetActive(true);
            roundResultText.text = "Damn, the AI won this round.";
            //increment if ai wins
            aiWins++;

            //Add 15 points to the AI's score
            StartCoroutine(AIRoundWinnerText());

        }
        else
        {
            roundResultText.gameObject.SetActive(true);
            roundResultText.text = "Oh! It's a tie... -_-??";
        }
    }

    //Co-routines that reveal the bonus points for the round winners.
    IEnumerator PlayerRoundWinnerText()
    {
        yield return new WaitForSeconds(2f);
        playerRoundWinnerText.gameObject.SetActive(true);
        playerRoundWinnerText.text = "Winner Bonus: " + roundWinner.ToString() + " pts";

        // Scale-up bounce using LeanTween
        LeanTween.scale(playerRoundWinnerText.gameObject, Vector3.one * 1.2f, 0.3f).setEaseOutBack()
            .setOnComplete(() => {
                // Slight shrink back to normal size
                LeanTween.scale(playerRoundWinnerText.gameObject, Vector3.one, 0.2f).setEaseOutExpo();
            });

        playerCurrency.AddPoints(roundWinner);

        yield return new WaitForSeconds(2f);
        playerRoundWinnerText.gameObject.SetActive(false);
    }

    IEnumerator AIRoundWinnerText()
    {
        yield return new WaitForSeconds(2f);
        aiRoundWinnerText.gameObject.SetActive(true);
        aiRoundWinnerText.text = "Winner Bonus: " + roundWinner.ToString() + " pts";

        // Scale-up bounce using LeanTween
        LeanTween.scale(aiRoundWinnerText.gameObject, Vector3.one * 1.2f, 0.3f).setEaseOutBack()
            .setOnComplete(() => {
                // Slight shrink back to normal size
                LeanTween.scale(aiRoundWinnerText.gameObject, Vector3.one, 0.2f).setEaseOutExpo();
            });

        yield return new WaitForSeconds(2f);
        aiRoundWinnerText.gameObject.SetActive(false);
    }

    void SetRandomTheme()
    {
        string[] themes = { "summer", "winter", "casual date", "artsy street style", "tv show audition" };
        int randomIndex = Random.Range(0, themes.Length);

        //Let's set the theme randomly to summer or WINTER. this is gonna be hard LOL
        currentTheme = themes[randomIndex];
        themeText.text = "THEME: " + currentTheme.ToUpper(); //The hell this mean?
    }

    void SetRandomAiPreference()
    {
        string[] tags = { "vibrant", "cozy", "chic", "flirty", "edgy", "bold", "warm" };
        int randomIndex = Random.Range(0, tags.Length);
        currentAiPreferredTag = tags[randomIndex];
        Debug.Log("AI preference for this round: " + currentAiPreferredTag);
        aiOutfitChanger.SetPreferredTag(currentAiPreferredTag);
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
    //public IEnumerator WaitAndStartNextRound()
    //{
    //    yield return new WaitForSeconds(1f);
    //    themePopupText.gameObject.SetActive(true);
    //    StartNextRound();
    //}

    void ResetRound()
    {
        //turn off the fast music and reset the round timer
        isFastMusicPlaying = false;
        roundTimer = 0f;
        //turn off the flash
        flashingStarted = false;

        //turn on the live score text in the clothing manager again
        clothingManager.playerLiveScoreText.gameObject.SetActive(true);
        clothingManager.aiLiveScoreText.gameObject.SetActive(true);

        //deactivate the final score text ui when a new round starts
        clothingManager.playerFinalScoreText.gameObject.SetActive(false);
        clothingManager.aiFinalScoreText.gameObject.SetActive(false);

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

        //randomly set the AI's tag preferences for the round
        SetRandomAiPreference();

        //Resetting the round countdown
        StartCoroutine (RoundCountdown());
    }

    //method to end the game
    void EndGame()
    {

        timerText.text = "Game Over \U0001F910"; //please don't be surprised future me. These are emojis in 'unicode' form
        scorePanel.SetActive(true);
        //Set the buttons active once the game has ended
        sceneUI.SetActive(true);
        //Make sure the next round button is hidden
        nextRoundButton.SetActive(false);

        //deactivate the final score text ui when a new round starts
        //clothingManager.playerFinalScoreText.gameObject.SetActive(false);
        //clothingManager.aiFinalScoreText.gameObject.SetActive(false);

        //display the amount of wins:
        clothingManager.playerFinalScoreText.text = "Total Player \nWins: " + playerWins.ToString() + " Wins";
        clothingManager.aiFinalScoreText.text = "Total AI \nWins: " + aiWins.ToString() + " Wins";

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

////something that will animate the bonus score reveal
//IEnumerator AnimateBonusScore(TextMeshProUGUI scoreText, int baseScore, int bonus, string label, float delay = 0.5f)
//{
//    Debug.Log("Coroutine started for " + label);

//    yield return new WaitForSeconds(delay);

//    int finalScore = baseScore + bonus;

//    // 🩷 Just set the final text once, no animation
//    scoreText.text = label + " Score: " + baseScore + " pts"
//        + "\nBonus: +" + bonus
//        + "\nTotal: " + finalScore + " pts";
//    Debug.Log("ScoreText is " + (scoreText == null ? "NULL 😭" : "NOT NULL 🎉"));
//    Debug.Log($"[AnimateBonusScore] Setting final score text for {label}!");

//    //scoreText.text = $"{label} Score: {baseScore} pts";
//    //yield return new WaitForSeconds(delay);

//    //scoreText.text += $"\n<color=#00FFC8>+{bonus} Style Bonus!</color>";
//    //yield return new WaitForSeconds(0.3f);

//    //int finalScore = baseScore + bonus;
//    //int current = baseScore;

//    //while (current < finalScore)
//    //{
//    //    current++;
//    //    scoreText.text = $"{label} Score: {current} pts\n<color=#00FFC8>+{bonus} Style Bonus!</color>";
//    //    scoreText.ForceMeshUpdate();
//    //    yield return new WaitForSeconds(0.05f);
//    //}
//    //Debug.Log("Animating score: " + current);
//}