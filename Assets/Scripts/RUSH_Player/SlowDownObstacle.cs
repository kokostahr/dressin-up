using System.Collections;
using UnityEngine;

public class SlowDownObstacle : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AIMovement AIMovement;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;
    public GameObject speedSlowed;
    public GameObject speedSlowedAI;

    private void Start()
    {
        speedSlowed.SetActive(false);
        speedSlowedAI.SetActive(false);
        Debug.Log("Obstacle Script Active");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Slowing down Player...");
            speedSlowed.SetActive(true);
            playerMovement.BoostSpeed(slowMultiplier, slowDuration);
            Destroy(gameObject);
            StartCoroutine(HideSpeedText());
        }
        else if (other.CompareTag("AI"))
        {
            Debug.Log("Slowing down AI...");
            speedSlowedAI.SetActive(true);
            AIMovement.BoostSpeed(slowMultiplier, slowDuration);
            Destroy(gameObject);
        }
    }

    //co-routine to make the text go away!
    IEnumerator HideSpeedText()
    {
        yield return new WaitForSeconds(1f);
        speedSlowed.SetActive(false);
        speedSlowedAI.SetActive(false);
    }
}
