using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    

    void Start()
    {
                InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
                

    }
    void SpawnEnemy()
    {
    Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), 0f, 15f);  // y 设置为 1f，确保敌人生成在地面上方
    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
