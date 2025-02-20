using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // �洢��ͬ�ȼ��ĵ���Prefab
    public Transform spawnArea; // ���������������ĵ�
    public float spawnInterval = 1f; // ���ɼ��ʱ�䣨����Inspector������
    public float spawnRangeX = 4f; // X�����ɷ�Χ

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
    // ÿ���Ѷ�ʱ������ spawnInterval ����ʼֵ��������ϵ��
    switch (difficultyLevel)
    {
        case 2:
            totalEnemies = 25;
            spawnInterval = spawnInterval * 0.9f;  // �����ʼֵ�� 1f������ϵ��
            break;
        case 3:
            totalEnemies = 25;
            spawnInterval = spawnInterval * 0.9f;  // �����ʼֵ�� 1f������ϵ��
            break;
        case 4:
            totalEnemies = 12;
            spawnInterval = spawnInterval * 0.9f;  // �����ʼֵ�� 1f������ϵ��
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
            // �������λ��
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX),
                spawnArea.position.y,
                spawnArea.position.z
            );

            GameObject enemyPrefab = SelectEnemyType();

            // ����ǵ����Ѷȣ�Boss����
            if (difficultyLevel == 4 && !bossSpawned)
            {
                GameObject boss = Instantiate(enemyPrefabs[3], spawnPos, Quaternion.identity); // Boss
                bossSpawned = true;
                i--; // ��������������֤��������
                continue;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // ���������ѡ���������
    GameObject SelectEnemyType()
    {
        int random = Random.Range(0, 100);

        return difficultyLevel switch
        {
            1 => random < 60 ? enemyPrefabs[0] : random < 90 ? enemyPrefabs[1] : enemyPrefabs[2],
            2 => random < 30 ? enemyPrefabs[0] : random < 80 ? enemyPrefabs[1] : enemyPrefabs[2],
            3 => random < 10 ? enemyPrefabs[0] : random < 45 ? enemyPrefabs[1] : enemyPrefabs[2],
            4 => enemyPrefabs[1], // Boss�Ѷ���Ҫ����LEVEL2
            _ => enemyPrefabs[0]
        };
    }

}
