using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float laneDistance = 2f; //this is the distance between each lane
    private int currentLane = 1; // 0= left land, 1= middle lane, 2= right lane
    private Vector3 targetPosition;


    void Start()
    {
        //when the game starts, the player must start in the middle lane
        targetPosition = transform.position;
    }

    
    void Update()
    {
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
    }
}
