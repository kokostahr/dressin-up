using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float laneDistance = 2f; //this is the distance between each lane
    private int currentLane = 1; // 0= left land, 1= middle lane, 2= right lane
    private Vector3 targetPosition;

    public float forwardSpeed = 5f; //basic movement speed
    public float speedMultiplier = 1.5f; //speed increase!
    private bool isMoving = true;
    private Vector3 startingPosition;

    void Start()
    {
        //when the game starts, the player must start in the middle lane
        currentLane = Random.Range(0, 3);
        startingPosition = transform.position;
        targetPosition = new Vector3(laneDistance * (currentLane - 1), transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (!isMoving) return;
        //Let's do the lane switching keyboard input
        if (Input.GetKeyDown(KeyCode.A) ||  Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > 0)
            {
                currentLane--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 2)
            {
                currentLane++;
            }
        }

        //calculate the target position based on the lane
        targetPosition = new Vector3(laneDistance * (currentLane - 1), transform.position.y, transform.position.z);
        //smoothly move to the next lane
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);

        //move forward constantly on the y axis
        transform.Translate(Vector3.up * forwardSpeed * speedMultiplier * Time.deltaTime);
    }

    //call this temporarily to boost the player's speed
    public void BoostSpeed(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private System.Collections.IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
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
