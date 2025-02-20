using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 存储不同等级的敌人Prefab
    public Transform spawnPoint; // 敌人生成点

    private int difficultyLevel = 1; // 初始难度等级
    private int totalEnemies = 25; // 每个难度生成的总敌人数量
    private float timeElapsed = 0f; // 游戏时间

    void Start()
    {
        StartCoroutine(SpawnEnemies()); // 开始协程生成敌人
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // 随着时间增加，提升游戏难度
        if (timeElapsed > 30f && difficultyLevel < 4)
        {
            difficultyLevel++;
            IncreaseDifficulty();
            timeElapsed = 0f; // 重置时间
        }
    }

    void IncreaseDifficulty()
    {
        // 难度提升后，根据比例重新生成敌人
        switch (difficultyLevel)
        {
            case 2:
                totalEnemies = 25; // 第二难度总敌人数量
                break;
            case 3:
                totalEnemies = 25; // 第三难度总敌人数量
                break;
            case 4:
                totalEnemies = 12; // 第四难度总敌人数量（减少敌人，增加Boss）
                break;
        }

        // 重新生成敌人
        StopCoroutine(SpawnEnemies()); // 停止之前的协程
        StartCoroutine(SpawnEnemies()); // 重新启动协程
    }

    // 协程生成敌人
    IEnumerator SpawnEnemies()
    {
        int level1Count = 0;
        int level2Count = 0;
        int level3Count = 0;
        int bossCount = 0;

        // 根据难度选择敌人种类比例
        switch (difficultyLevel)
        {
            case 1:
                level1Count = Mathf.RoundToInt(totalEnemies * 0.6f); // LEVEL1 60%
                level2Count = Mathf.RoundToInt(totalEnemies * 0.3f); // LEVEL2 30%
                level3Count = Mathf.RoundToInt(totalEnemies * 0.1f); // LEVEL3 10%
                break;
            case 2:
                level1Count = Mathf.RoundToInt(totalEnemies * 0.3f); // LEVEL1 30%
                level2Count = Mathf.RoundToInt(totalEnemies * 0.5f); // LEVEL2 50%
                level3Count = Mathf.RoundToInt(totalEnemies * 0.2f); // LEVEL3 20%
                break;
            case 3:
                level1Count = Mathf.RoundToInt(totalEnemies * 0.1f); // LEVEL1 10%
                level2Count = Mathf.RoundToInt(totalEnemies * 0.35f); // LEVEL2 35%
                level3Count = Mathf.RoundToInt(totalEnemies * 0.55f); // LEVEL3 55%
                break;
            case 4:
                level2Count = Mathf.RoundToInt(totalEnemies * 0.9f); // LEVEL2 90%
                bossCount = 1; // 1个Boss
                break;
        }

        // 生成敌人，并控制生成速度
        for (int i = 0; i < level1Count; i++)
        {
            Instantiate(enemyPrefabs[0], spawnPoint.position, Quaternion.identity); // LEVEL1
            yield return new WaitForSeconds(0.05f); // 等待0.1秒再生成下一个敌人
        }
        for (int i = 0; i < level2Count; i++)
        {
            Instantiate(enemyPrefabs[1], spawnPoint.position, Quaternion.identity); // LEVEL2
            yield return new WaitForSeconds(0.05f); // 等待0.1秒再生成下一个敌人
        }
        for (int i = 0; i < level3Count; i++)
        {
            Instantiate(enemyPrefabs[2], spawnPoint.position, Quaternion.identity); // LEVEL3
            yield return new WaitForSeconds(0.05f); // 等待0.1秒再生成下一个敌人
        }
        for (int i = 0; i < bossCount; i++)
        {
            Instantiate(enemyPrefabs[3], spawnPoint.position, Quaternion.identity); // Boss
            yield return new WaitForSeconds(0.05f); // 等待0.1秒再生成下一个敌人
        }
    }
}
