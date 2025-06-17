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

    void Start()
    {
        currentLane = Random.Range(0, 3);
        targetPosition = new Vector3(laneDistance * (currentLane - 1), transform.position.y, transform.position.z);
        decisionTimer = decisionInterval;
    }

    void Update()
    {
        // Timer for AI decisions
        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            MakeRandomLaneDecision();
            decisionTimer = decisionInterval;
        }

        // Movement like player
        targetPosition = new Vector3(laneDistance * (currentLane - 1), transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);

        // Move forward
        transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);
    }

    void MakeRandomLaneDecision()
    {
        int direction = Random.Range(-1, 2); // -1, 0, or 1
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }
}
