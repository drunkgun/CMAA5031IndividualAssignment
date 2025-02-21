using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 单例实例
    public static GameManager instance;

    [System.Serializable]
    public class DifficultySettings
    {
        public int baseEnemyCount;    // 基础敌人数（不含Boss）
        public float spawnInterval;
        public bool spawnBoss;
    }

    [Header("配置参数")]
    [SerializeField] private DifficultySettings[] difficultySettings;
    [SerializeField] private GameObject[] enemyPrefabs; // 0:小 1:中 2:大 3:Boss
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Transform spawnArea;
    [SerializeField][Range(2f, 10f)] private float spawnRangeX = 4f;

    [Header("运行时状态")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int remainingEnemies;
    private bool isSpawning = false;
    private bool bossSpawnedThisWave = false;
    private float waveStartTime;

    // 单例模式
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 保证GameManager在场景切换时不被销毁
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        gameOverUI.SetActive(false);
        StartNewWave();
    }

    private void StartNewWave()
    {
        if (currentWave >= difficultySettings.Length) return;

        remainingEnemies = difficultySettings[currentWave].baseEnemyCount;
        bossSpawnedThisWave = false;
        waveStartTime = Time.time; // 设置波次开始时间
        StartCoroutine(EnemySpawnRoutine(difficultySettings[currentWave]));
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

        // 生成Boss（如果配置需要）
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
        remainingEnemies++; // 补偿Boss的计数
    }

    private void SpawnNormalEnemy()
    {
        GameObject prefab = SelectEnemyType();
        Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            spawnArea.position.y,
            spawnArea.position.z
        );
    }

    public void OnEnemyDefeated(bool isBoss)
    {
        remainingEnemies--;
        if (isBoss) remainingEnemies--; // 修正Boss双倍计数问题

        if (remainingEnemies <= 0 && !isSpawning)
        {
            HandleWaveCompletion();
        }
    }

    private void HandleWaveCompletion()
    {
        currentWave = Mathf.Min(currentWave + 1, difficultySettings.Length - 1);
        if (currentWave >= difficultySettings.Length)
        {
            TriggerVictory();
        }
        else
        {
            StartNewWave();
        }
    }

    private void TriggerVictory()
    {
        Debug.Log("All waves cleared!");
        // 可添加胜利界面逻辑
    }

    private void Update()
    {
        if (currentWave < difficultySettings.Length - 1)
        {
            CheckWaveProgression();
        }
    }

    private void CheckWaveProgression()
    {
        if (Time.time - waveStartTime > (currentWave + 1) * 30f) // 每波最大持续时间
        {
            ProgressToNextWave();
        }
    }

    private void ProgressToNextWave()
    {
        currentWave = Mathf.Min(currentWave + 1, difficultySettings.Length - 1);
        waveStartTime = Time.time; // 重置每波开始时间
        StartNewWave();
    }

    private GameObject SelectEnemyType()
    {
        if (enemyPrefabs.Length < 3)
        {
            Debug.LogError("至少需要3种基础敌人预制体");
            return null;
        }

        float random = Random.value;
        int prefabIndex = 0;

        if (currentWave == 0)
        {
            prefabIndex = random < 0.6f ? 0 : random < 0.9f ? 1 : 2;
        }
        else if (currentWave == 1)
        {
            prefabIndex = random < 0.3f ? 0 : random < 0.8f ? 1 : 2;
        }
        else // currentWave >= 2
        {
            prefabIndex = random < 0.1f ? 0 : random < 0.45f ? 1 : 2;
        }

        return enemyPrefabs[prefabIndex];
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        StopAllCoroutines();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 新增调试功能
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Current Wave: {currentWave + 1}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Remaining Enemies: {remainingEnemies}");
    }
}
