using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private int numberToSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        for (var i = 0; i < this.numberToSpawn; ++i)
            Instantiate(this.obstacle, Random.insideUnitSphere * 100f, Quaternion.identity);
    }
}
