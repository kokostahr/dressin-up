using System.Collections;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float laneDistance = 2f;
    private int currentLane = 1;
    private Vector3 targetPosition;

    public float forwardSpeed = 5f;
    public float decisionInterval = 2f;
    private float decisionTimer;

    public float speedMultiplier = 1f;
    private bool isMoving = true;
    private Vector3 startingPosition;

    void Start()
    {
        currentLane = Random.Range(0, 3);
        startingPosition = transform.position;
        targetPosition = new Vector3(laneDistance * (currentLane - 1), transform.position.y, transform.position.z);
        decisionTimer = decisionInterval;
    }

    void Update()
    {
        if (!isMoving) return;

        // Timer for AI decisions
        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            ChooseLaneBasedOnClothes();
            decisionTimer = decisionInterval;
        }

        // Movement like player
        targetPosition = new Vector3(laneDistance * (currentLane - 1), transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);

        // Move forward
        transform.Translate(Vector3.up * forwardSpeed * speedMultiplier * Time.deltaTime);
    }

    void ChooseLaneBasedOnClothes()
    {
        GameObject[] clothes = GameObject.FindGameObjectsWithTag("Clothing");
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject item in clothes)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (item.transform.position.y > transform.position.y && dist < closestDist)
            {
                closest = item;
                closestDist = dist;
            }
        }

        if (closest != null)
        {
            float itemX = closest.transform.position.x;

            if (itemX < -0.5f)
                currentLane = 0;
            else if (itemX > 0.5f)
                currentLane = 2;
            else
                currentLane = 1;
        }
        else
        {
            // fallback random decision if nothing nearby
            MakeRandomLaneDecision();
        }
    }

    void MakeRandomLaneDecision()
    {
        int direction = Random.Range(-1, 2); // -1, 0, or 1
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }

    public void BoostSpeed(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        Debug.Log("Speed Boost STARTED");
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
        Debug.Log("Speed Boost ENDED");
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void ResumeMoving()
    {
        isMoving = true;
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
        currentLane = 1;
        targetPosition = new Vector3(laneDistance * (currentLane - 1), startingPosition.y, startingPosition.z);
    }
}
