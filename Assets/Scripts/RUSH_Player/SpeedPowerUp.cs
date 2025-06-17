using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float boostDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>()?.BoostSpeed(speedMultiplier, boostDuration);
            Destroy(gameObject);
        }
        else if (other.CompareTag("AI"))
        {
            other.GetComponent<AIMovement>()?.BoostSpeed(speedMultiplier, boostDuration);
            Destroy(gameObject);
        }
    }
}
