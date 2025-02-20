using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 引入UI命名空间



public class Enemy : MonoBehaviour
{
    public int level; // 敌人等级
    public int health; // 敌人血量
    public Slider healthBar; // 血条 Slider
    public float speed = 5f;

    private void Start()
    {
        // 直接使用 Prefab 中的 health 值，不再在脚本中设置
        if (healthBar != null)
        {
            healthBar.maxValue = health; // 设置血条最大值
            healthBar.value = health; // 设置血条当前值
        }
    }


    private void Update()
    {
        // 移动
        transform.position += Vector3.back * speed * Time.deltaTime;

        // 超出视野销毁
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }

    // 子弹碰撞时减少血量
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            int bulletPower = other.GetComponent<Bullet>().power; // 获取子弹威力

            // 输出原始的 health 和子弹的 power
            Debug.Log("Enemy hit! Original health: " + health + " | Bullet power: " + bulletPower);

            // 扣除血量
            health -= bulletPower; 

            // 输出更新后的 health
            Debug.Log("Health reduced by " + bulletPower + ". Current health: " + health);

            // 如果血量小于等于0，销毁敌人
            if (health <= 0)
            {
                Debug.Log("Enemy destroyed");
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
}
