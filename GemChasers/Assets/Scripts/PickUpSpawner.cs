using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    public GameObject healthPickup;
    public List<GameObject> activeHealthPickups;
    public GameObject energyPickup;
    public List<GameObject> activeEnergyPickups;
    int numHealthPickups;
    float lastSpawned = 0;
    float spawnDelay = 3;
    public float maxX;
    public float minX;
    public float maxZ;
    public float minZ;
    public float yVal;
    void Start()
    {
        
    }


    void Update()
    {
        if (Time.timeSinceLevelLoad - lastSpawned > spawnDelay) 
        {
            lastSpawned = Time.timeSinceLevelLoad;
            int num = Random.Range(0, 2);
            float ranX = Random.Range(minX, maxX);
            float ranZ = Random.Range(minZ, maxZ);

            bool spawned = false;
            if (num == 0) 
            {
                spawned = AttemptSpawnHealth(ranX, ranZ);
                if (!spawned) 
                {
                    AttemptSpawnEnergy(ranX, ranZ);
                }
            }
            else
            {
                spawned = AttemptSpawnEnergy(ranX, ranZ);
                if (!spawned) 
                {
                    AttemptSpawnHealth(ranX, ranZ);
                }
            }
        }
    }
    bool AttemptSpawnHealth(float x, float z)  
    {
        bool spawned = false;
        if (activeHealthPickups.Count < 4)
        {
            GameObject obj = Instantiate(healthPickup, new Vector3(x, yVal, z), Quaternion.identity);
            obj.GetComponent<Pickup>().spawner = this;
            activeHealthPickups.Add(obj);
            
            spawned = true;
        }
        return spawned;
    }
    bool AttemptSpawnEnergy(float x, float z) 
    {
        bool spawned = false;
        if (activeEnergyPickups.Count < 4)
        {
            GameObject obj = Instantiate(energyPickup, new Vector3(x, yVal, z), Quaternion.identity);
            obj.GetComponent<Pickup>().spawner = this;
            activeEnergyPickups.Add(obj);

            spawned = true;
        }
        return spawned;
    }
}
