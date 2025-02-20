using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 存储不同等级的敌人Prefab
    public Transform spawnArea; // 敌人生成区域中心点
    public float spawnInterval = 1f; // 生成间隔时间（可在Inspector调整）
    public float spawnRangeX = 4f; // X轴生成范围

    private int difficultyLevel = 1;
    private int totalEnemies = 25;
    private float timeElapsed = 0f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > 30f && difficultyLevel < 4)
        {
            difficultyLevel++;
            IncreaseDifficulty();
            timeElapsed = 0f;
        }
    }

void IncreaseDifficulty()
{
    // 每个难度时，重置 spawnInterval 到初始值，并乘以系数
    switch (difficultyLevel)
    {
        case 2:
            totalEnemies = 25;
            spawnInterval = spawnInterval * 0.9f;  // 假设初始值是 1f，乘以系数
            break;
        case 3:
            totalEnemies = 25;
            spawnInterval = spawnInterval * 0.9f;  // 假设初始值是 1f，乘以系数
            break;
        case 4:
            totalEnemies = 12;
            spawnInterval = spawnInterval * 0.9f;  // 假设初始值是 1f，乘以系数
            break;
    }

    StopCoroutine(SpawnEnemies());  // Stop the currently running SpawnEnemies coroutine
    StartCoroutine(SpawnEnemies()); // Restart the coroutine with new difficulty settings
}

    IEnumerator SpawnEnemies()
    {
        bool bossSpawned = false;

        for (int i = 0; i < totalEnemies; i++)
        {
            // 生成随机位置
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX),
                spawnArea.position.y,
                spawnArea.position.z
            );

            GameObject enemyPrefab = SelectEnemyType();

            // 如果是第四难度，Boss生成
            if (difficultyLevel == 4 && !bossSpawned)
            {
                GameObject boss = Instantiate(enemyPrefabs[3], spawnPos, Quaternion.identity); // Boss
                bossSpawned = true;
                i--; // 补偿计数器，保证总数不变
                continue;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 根据随机数选择敌人类型
    GameObject SelectEnemyType()
    {
        int random = Random.Range(0, 100);

        return difficultyLevel switch
        {
            1 => random < 60 ? enemyPrefabs[0] : random < 90 ? enemyPrefabs[1] : enemyPrefabs[2],
            2 => random < 30 ? enemyPrefabs[0] : random < 80 ? enemyPrefabs[1] : enemyPrefabs[2],
            3 => random < 10 ? enemyPrefabs[0] : random < 45 ? enemyPrefabs[1] : enemyPrefabs[2],
            4 => enemyPrefabs[1], // Boss难度主要生成LEVEL2
            _ => enemyPrefabs[0]
        };
    }

}
