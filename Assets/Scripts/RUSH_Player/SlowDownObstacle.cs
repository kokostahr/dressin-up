using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlowDownObstacle : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AIMovement AIMovement;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;
    public GameObject speedSlowedText;

    private void Start()
    {
        speedSlowedText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speedSlowedText.SetActive(true);
            playerMovement.BoostSpeed(slowMultiplier, slowDuration);
            Destroy(gameObject);
            StartCoroutine(HideSpeedText());
        }
        else if (other.CompareTag("AI"))
        {
            AIMovement.BoostSpeed(slowMultiplier, slowDuration);
            Destroy(gameObject);
        }
    }

    //co-routine to make the text go away!
    IEnumerator HideSpeedText()
    {
        yield return new WaitForSeconds(1f);
        speedSlowedText.gameObject.SetActive(false);
    }
}
