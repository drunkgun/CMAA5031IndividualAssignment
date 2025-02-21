using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // 添加此行以使用 UI 元素，如 Button


public class GameManager : MonoBehaviour
{
    // 单例实例
    public static GameManager instance;

    [System.Serializable]
    public class DifficultySettings
    {
        public int baseEnemyCount;    // 基础敌人数（不含Boss）
        public float spawnInterval;   // 敌人生成的时间间隔
        public bool spawnBoss;        // 是否生成Boss
        public float rewardMultiplier = 1.3f; // 奖励倍数

    }

    [Header("Setting Here")]
    [SerializeField] private DifficultySettings[] difficultySettings;
    [SerializeField] private GameObject[] enemyPrefabs; // 0:小 1:中 2:大 3:Boss
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Transform spawnArea;
    [SerializeField][Range(2f, 10f)] private float spawnRangeX = 4f;

    [Header("Skill UI")]
    [SerializeField] private GameObject skillUI; // 显示技能加成按钮的UI
    [SerializeField] private Button bulletPowerButton; // 子弹威力按钮
    [SerializeField] private Button bulletSpeedButton; // 子弹速度按钮

    [Header("Running Status")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int remainingEnemies;
    private bool isSpawning = false;
    private bool bossSpawnedThisWave = false;
    private float waveStartTime;
    public float bulletPower = 1f;  // 初始子弹威力
    public float bulletSpeed = 20f;  // 初始子弹速度

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
        skillUI.SetActive(false); // 开始时隐藏技能按钮

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
            yield return new WaitForSeconds(settings.spawnInterval);  // 延时生成每个敌人
        }

        // 生成Boss（如果配置需要）
        if (settings.spawnBoss && !bossSpawnedThisWave)
        {
            SpawnBoss();
        }

        isSpawning = false;

        // 每波结束后暂停游戏并显示技能界面
        ShowSkillButtons();
        Time.timeScale = 0f;  // 暂停游戏
    }

    private void ShowSkillButtons()
    {
        skillUI.SetActive(true); // 显示技能加成按钮

        // 绑定按钮点击事件
        bulletPowerButton.onClick.AddListener(UpgradeBulletPower);
        bulletSpeedButton.onClick.AddListener(UpgradeBulletSpeed);
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
    // 升级子弹威力
    private void UpgradeBulletPower()
    {
        bulletPower *= difficultySettings[currentWave].rewardMultiplier;
        Debug.Log($"Bullet Power Upgraded: {bulletPower}");
        skillUI.SetActive(false); // 隐藏技能按钮
        Time.timeScale = 1f; // 恢复游戏
    }

    // 升级子弹速度
    private void UpgradeBulletSpeed()
    {
        bulletSpeed *= difficultySettings[currentWave].rewardMultiplier;
        Debug.Log($"Bullet Speed Upgraded: {bulletSpeed}");
        skillUI.SetActive(false); // 隐藏技能按钮
        Time.timeScale = 1f; // 恢复游戏
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
