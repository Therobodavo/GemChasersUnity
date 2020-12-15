using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Enemy Spawner Class
 * Programmed by David Knolls
 * 
 * Spawns enemies onto a path in the world
 */

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Path path;
    public float spawnDelay = 3;
    private float lastSpawnTime = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If a path has been set
        if (path) 
        {
            if (Time.timeSinceLevelLoad - lastSpawnTime > spawnDelay) 
            {
                //Spawn Enemy
                lastSpawnTime = Time.timeSinceLevelLoad;

                //Randomly choose a prefab from the list provided
                int ranNum = Random.Range(0, enemyPrefabs.Length);
                GameObject e = Instantiate(enemyPrefabs[ranNum], path.start.transform.position, path.start.transform.rotation);

                Enemy eScript = e.GetComponent<Enemy>();
                eScript.path = this.path;
                eScript.currentPathTarget = eScript.path.start;
                eScript.spawnTime = Time.timeSinceLevelLoad;
            }
        }
    }
}
