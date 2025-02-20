using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // �洢��ͬ�ȼ��ĵ���Prefab
    public Transform spawnPoint; // �������ɵ�

    private int difficultyLevel = 1; // ��ʼ�Ѷȵȼ�
    private int totalEnemies = 25; // ÿ���Ѷ����ɵ��ܵ�������
    private float timeElapsed = 0f; // ��Ϸʱ��

    void Start()
    {
        StartCoroutine(SpawnEnemies()); // ��ʼЭ�����ɵ���
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // ����ʱ�����ӣ�������Ϸ�Ѷ�
        if (timeElapsed > 30f && difficultyLevel < 4)
        {
            difficultyLevel++;
            IncreaseDifficulty();
            timeElapsed = 0f; // ����ʱ��
        }
    }

    void IncreaseDifficulty()
    {
        // �Ѷ������󣬸��ݱ����������ɵ���
        switch (difficultyLevel)
        {
            case 2:
                totalEnemies = 25; // �ڶ��Ѷ��ܵ�������
                break;
            case 3:
                totalEnemies = 25; // �����Ѷ��ܵ�������
                break;
            case 4:
                totalEnemies = 12; // �����Ѷ��ܵ������������ٵ��ˣ�����Boss��
                break;
        }

        // �������ɵ���
        StopCoroutine(SpawnEnemies()); // ֹ֮ͣǰ��Э��
        StartCoroutine(SpawnEnemies()); // ��������Э��
    }

    // Э�����ɵ���
    IEnumerator SpawnEnemies()
    {
        int level1Count = 0;
        int level2Count = 0;
        int level3Count = 0;
        int bossCount = 0;

        // �����Ѷ�ѡ������������
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
                bossCount = 1; // 1��Boss
                break;
        }

        // ���ɵ��ˣ������������ٶ�
        for (int i = 0; i < level1Count; i++)
        {
            Instantiate(enemyPrefabs[0], spawnPoint.position, Quaternion.identity); // LEVEL1
            yield return new WaitForSeconds(0.05f); // �ȴ�0.1����������һ������
        }
        for (int i = 0; i < level2Count; i++)
        {
            Instantiate(enemyPrefabs[1], spawnPoint.position, Quaternion.identity); // LEVEL2
            yield return new WaitForSeconds(0.05f); // �ȴ�0.1����������һ������
        }
        for (int i = 0; i < level3Count; i++)
        {
            Instantiate(enemyPrefabs[2], spawnPoint.position, Quaternion.identity); // LEVEL3
            yield return new WaitForSeconds(0.05f); // �ȴ�0.1����������һ������
        }
        for (int i = 0; i < bossCount; i++)
        {
            Instantiate(enemyPrefabs[3], spawnPoint.position, Quaternion.identity); // Boss
            yield return new WaitForSeconds(0.05f); // �ȴ�0.1����������һ������
        }
    }
}
