using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Path path;
    public float spawnDelay = 3;
    private float lastSpawnTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (path) 
        {
            if (Time.timeSinceLevelLoad - lastSpawnTime > spawnDelay) 
            {
                lastSpawnTime = Time.timeSinceLevelLoad;
                int ranNum = Random.Range(0, 2);
                GameObject e = Instantiate(enemyPrefabs[ranNum], path.start.transform.position, path.start.transform.rotation);
                Enemy eScript = e.GetComponent<Enemy>();
                eScript.path = this.path;
                eScript.currentPathTarget = eScript.path.start;
                eScript.spawnTime = Time.timeSinceLevelLoad;
            }
        }
    }
}
