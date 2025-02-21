using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // 引入UI命名空间

public class Enemy : MonoBehaviour
{
    public bool isBoss = false; // 新增字段

    // 公共字段
    public int level; // 敌人等级
    public float health; // 敌人血量
    public Slider healthBar; // 血条 Slider
    public float speed = 5f; // 敌人移动速度

    // 私有字段
    private Transform playerTransform; // 玩家位置
    private bool hasCheckedPosition = false; // 是否已检查过敌人与玩家位置

    // 游戏开始时调用
    private void Start()
    {
        // 获取玩家的Transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 初始化血条
        if (healthBar != null)
        {
            healthBar.maxValue = health; // 设置血条最大值
            healthBar.value = health; // 设置血条当前值
        }

        // 确保敌人拥有刚体组件
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    // 每帧更新时调用
    private void Update()
    {
        // 移动敌人
        transform.position += Vector3.back * speed * Time.deltaTime;

        // 超出视野销毁敌人
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }

        // 如果敌人已接近玩家并且尚未检查过
        if (!hasCheckedPosition && transform.position.z < playerTransform.position.z)
        {
            GameManager.instance.TriggerGameOver(); // 调用GameOver
            hasCheckedPosition = true; // 标记已检查过
        }
    }

    // 子弹与敌人发生碰撞时
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            float bulletPower = other.GetComponent<Bullet>().power; // 获取子弹威力

            // 扣除血量
            health -= bulletPower;

            // 如果血量小于等于0，销毁敌人
            if (health <= 0)
            {
                Destroy(gameObject); // 血量为0销毁敌人
                ScoreManager.instance?.AddScore(10); // 增加分数
            }

            // 更新血条
            if (healthBar != null)
            {
                healthBar.value = Mathf.Max(health, 0); // 确保血条值不小于0
            }
        }
    }

    // 玩家与敌人发生碰撞时
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.TriggerGameOver(); // 调用GameOver
        }
    }

    // 销毁敌人时调用
    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnEnemyDefeated(isBoss);
        }
    }
}
