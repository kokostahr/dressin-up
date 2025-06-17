using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] clothesPrefabs;
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 3f; //every few seconds, clothes should popup
    public float spawnZ = 10f; //this is the distance infront of the player
    public float laneDistance = 2f;

    public Transform player;

    public float timer = 0f;

    private void Start()
    {
        HideAll(obstaclePrefabs);
        HideAll(clothesPrefabs);
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
        float dynamicZ = player.position.z + spawnZ; // spawns ahead of the player
        Vector3 spawnPos = new Vector3(xPos, 0f, dynamicZ);

        // Decide if it's clothes or obstacle
        bool spawnClothes = Random.value > 0.5f;

        GameObject prefabToSpawn;

        if (spawnClothes && clothesPrefabs.Length > 0)
        {
            prefabToSpawn = clothesPrefabs[Random.Range(0, clothesPrefabs.Length)];
        }
        else if (obstaclePrefabs.Length > 0)
        {
            prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        }
        else
        {
            return; // Nothing to spawn
        }

        // Instantiate and make sure it's active!
        GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        spawnedItem.SetActive(true);// important if original was deactivated
    }
}
