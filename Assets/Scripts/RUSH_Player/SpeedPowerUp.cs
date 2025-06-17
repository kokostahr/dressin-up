using System.Collections;
using TMPro;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AIMovement AIMovement;
    public float speedMultiplier = 2f;
    public float boostDuration = 3f;
    public GameObject speedIncreased;
    public GameObject speedIncreasedAI;

    private void Start()
    {
        speedIncreased.SetActive(false);
        speedIncreasedAI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            speedIncreased.SetActive(true);
            playerMovement.BoostSpeed(speedMultiplier, boostDuration);
            Destroy(gameObject);
            StartCoroutine(HideSpeedText());
        }
        else if (other.CompareTag("AI"))
        {
            speedIncreasedAI.SetActive(true);
            AIMovement.BoostSpeed(speedMultiplier, boostDuration);
            Destroy(gameObject);
            StartCoroutine(HideSpeedText());
        }
    }

    //co-routine to make the text go away!
    IEnumerator HideSpeedText()
    {
        yield return new WaitForSeconds(0.5f);
        speedIncreased.SetActive(false);
        speedIncreasedAI.SetActive(false);
    }
}
