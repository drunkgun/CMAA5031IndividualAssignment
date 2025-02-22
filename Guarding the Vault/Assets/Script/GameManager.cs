using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [System.Serializable]
    public class DifficultySettings
    {
        public int baseEnemyCount;
        public float spawnInterval;
        public bool spawnBoss;
        public float rewardMultiplier = 1.3f;
    }

    [Header("Setting Here")]
    [SerializeField] private DifficultySettings[] difficultySettings;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform spawnArea;
    [SerializeField][Range(2f, 10f)] private float spawnRangeX = 4f;

    [Header("Running Status")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int remainingEnemies;
    private bool isSpawning = false;
    private bool bossSpawnedThisWave = false;
    private float waveStartTime;

    // 统一管理子弹属性
    public float bulletPower = 1f;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        // 重置所有状态变量
// 重置所有状态变量
    currentWave = 0;
    bulletPower = 1f;
    bulletSpeed = 20f;
    fireRate = 0.5f;
    remainingEnemies = 0; // 确保重置为 0
    isSpawning = false;
    bossSpawnedThisWave = false;

        // 隐藏所有 UI
        UIManager.instance.HideAllUI();
    // 绑定技能按钮事件
    UIManager.instance.BindSkillButtons(
        () => UpgradeSkill(() => UpgradeBulletPower(currentWave)),
        () => UpgradeSkill(() => UpgradeFireRate(currentWave))
    );

    // 绑定 Play Again 按钮事件
    UIManager.instance.BindPlayAgainButton(RestartGame);


        // 清理场景中的敌人和子弹
        ClearSceneObjects();

        // 开始新波次
        StartNewWave();
    }

    private void ClearSceneObjects()
    {
        // 销毁所有敌人
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // 销毁所有子弹
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    private void StartNewWave()
    {
        if (currentWave >= difficultySettings.Length)
        {
            TriggerVictory();
            return;
        }

        var settings = difficultySettings[currentWave];
        remainingEnemies = settings.baseEnemyCount;
        if (settings.spawnBoss) remainingEnemies += 1;

        bossSpawnedThisWave = false;
        waveStartTime = Time.time;
        StartCoroutine(EnemySpawnRoutine(settings));
    }

    private IEnumerator EnemySpawnRoutine(DifficultySettings settings)
    {
        isSpawning = true;

        // 生成基础敌人
        for (int i = 0; i < settings.baseEnemyCount; i++)
        {
            SpawnNormalEnemy();
            yield return new WaitForSeconds(settings.spawnInterval);
        }

        // 生成Boss
        if (settings.spawnBoss && !bossSpawnedThisWave)
        {
            SpawnBoss();
        }

        isSpawning = false;
    }

    private void SpawnBoss()
    {
        Instantiate(enemyPrefabs[3], GetSpawnPosition(), Quaternion.identity);
        bossSpawnedThisWave = true;
    }

    private void SpawnNormalEnemy()
    {
        GameObject prefab = SelectEnemyType();
        if (prefab != null)
        {
            Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            spawnArea.position.y,
            spawnArea.position.z
        );
    }

    private GameObject SelectEnemyType()
    {
        if (enemyPrefabs.Length < 3)
        {
            Debug.LogError("至少需要3种基础敌人预制体");
            return null;
        }

        float random = UnityEngine.Random.value;
        int prefabIndex = 0;

        if (currentWave == 0)
        {
            prefabIndex = random < 0.6f ? 0 : random < 0.9f ? 1 : 2;
        }
        else if (currentWave == 1)
        {
            prefabIndex = random < 0.3f ? 0 : random < 0.8f ? 1 : 2;
        }
        else
        {
            prefabIndex = random < 0.1f ? 0 : random < 0.45f ? 1 : 2;
        }

        return enemyPrefabs[prefabIndex];
    }

    public void OnEnemyDefeated(bool isBoss)
    {
        remainingEnemies--;

        if (remainingEnemies <= 0 && !isSpawning)
        {
            if (currentWave >= difficultySettings.Length - 1)
            {
                TriggerVictory();
            }
            else
            {
                UIManager.instance.ShowSkillButtons();
                Time.timeScale = 0f;
            }
        }
    }

  private void UpgradeSkill(Action upgradeAction)
{
    upgradeAction(); // 执行升级操作
    UIManager.instance.HideSkillButtons();
    Time.timeScale = 1f;
    currentWave++;
    StartNewWave();
}

private void UpgradeBulletPower(int waveIndex)
{
    bulletPower *= difficultySettings[waveIndex].rewardMultiplier;
    Debug.Log($"Bullet Power Upgraded to: {bulletPower}");
}

private void UpgradeFireRate(int waveIndex)
{
    fireRate /= difficultySettings[waveIndex].rewardMultiplier;
    Debug.Log($"Fire Rate Upgraded to: {fireRate}");
}

    public void TriggerGameOver()
    {
        Debug.Log("Game Over!");
        UIManager.instance.ShowResultUI(true);
        Time.timeScale = 0f;
    }

    private void TriggerVictory()
    {
        Debug.Log("All waves cleared!");
        UIManager.instance.ShowResultUI(false);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // 重置时间缩放
        Time.timeScale = 1f;

        // 重置游戏状态
        InitializeGame();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Current Wave: {currentWave + 1}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Remaining Enemies: {remainingEnemies}");
    }
}