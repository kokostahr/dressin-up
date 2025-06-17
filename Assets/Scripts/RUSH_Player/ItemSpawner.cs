using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] clothesPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject[] speedPowerUpPrefab;
    public float spawnInterval = 3f; //every few seconds, clothes should popup
    public float spawnZ = 10f; //this is the distance infront of the player
    public float laneDistance = 2f;

    public Transform player;

    public float timer = 0f;
    private bool isSpawning = true;

    private void Start()
    {
        HideAll(obstaclePrefabs);
        HideAll(clothesPrefabs);
        HideAll(speedPowerUpPrefab);
    }

    void HideAll(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
    }

    void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomItem();
            timer = 0f;
        }
    }

    void SpawnRandomItem()
    {
        int lane = Random.Range(0, 3); // 3 lanes: 0,1,2
        float xPos = laneDistance * (lane - 1);
        float dynamicY = player.position.y + spawnZ; // now spawns above the player
        Vector3 spawnPos = new Vector3(xPos, dynamicY, 0f); // Y instead of Z!

        GameObject prefabToSpawn;
        // Add a third possibility: 10% chance to spawn powerup
        float roll = Random.value;

        if (roll < 0.5f && clothesPrefabs.Length > 0)//50%
        {
            prefabToSpawn = clothesPrefabs[Random.Range(0, clothesPrefabs.Length)];
        }
        else if (roll < 0.8f && obstaclePrefabs.Length > 0)//30%
        {
            prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        }
        else if (speedPowerUpPrefab.Length > 0)//20
        {
            prefabToSpawn = speedPowerUpPrefab[Random.Range(0, speedPowerUpPrefab.Length)];
        }
        else
        {
            return;
        }

        // Instantiate and make sure it's active!
        GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        spawnedItem.SetActive(true);// important if original was deactivated
    }

    // ✋ STOP THE SPAWN MADNESS!
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // 🔄 RESET AND RESTART THE PARTY
    public void ResetSpawner()
    {
        timer = 0f;
        isSpawning = true;
    }
}
