using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public bool isBoss = false; // 是否是 Boss

    // 公共字段
    public int level; // 敌人等级
    public float health; // 敌人血量
    public Slider healthBar; // 血条 Slider
    public float speed = 5f; // 敌人移动速度
    private Transform playerTransform; // 玩家位置

    // 游戏开始时调用
    private void Start()
    {
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
                // 获取玩家的Transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // 每帧更新时调用
    private void Update()
    {
        // 移动敌人
        transform.position += Vector3.back * speed * Time.deltaTime;
        // 检查敌人是否通过玩家防线
        if (transform.position.z < playerTransform.position.z)
        {
            // 敌人通过防线，触发游戏结束
            GameManager.instance.TriggerGameOver();
            Destroy(gameObject); // 销毁敌人
        }
        // 超出视野销毁敌人
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }

    // 子弹与敌人发生碰撞时
private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Bullet"))
    {
        // 从 GameManager 获取子弹威力
        float bulletPower = GameManager.instance.bulletPower;

        // 扣除血量
        health -= bulletPower;

        // 如果血量小于等于0，销毁敌人
        if (health <= 0)
        {
            Destroy(gameObject); // 血量为0销毁敌人

            // 调用 ScoreManager 增加分数
            ScoreManager.instance?.AddScore(isBoss ? 100 : 10); // 使用空值传播避免空引用
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
            // 玩家与敌人碰撞时，通知 GameManager 游戏结束
            GameManager.instance.TriggerGameOver();
        }
    }

    // 销毁敌人时调用
    private void OnDestroy()
    {
        // 通知 GameManager 敌人被击败
        GameManager.instance.OnEnemyDefeated(isBoss);
    }
}