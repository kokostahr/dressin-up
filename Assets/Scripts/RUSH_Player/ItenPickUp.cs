using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ItenPickUp : MonoBehaviour
{
    private int playerScore = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Clothing"))
        {
            playerScore += 10; // or whatever points
            Destroy(other.gameObject);
            Debug.Log("Clothing picked up! Score: " + playerScore);
        }
        else if (other.CompareTag("Obstacle"))
        {
            playerScore -= 5; // penalty maybe
            Destroy(other.gameObject);
            Debug.Log("Ouch! Hit obstacle. Score: " + playerScore);
        }
    }
}
