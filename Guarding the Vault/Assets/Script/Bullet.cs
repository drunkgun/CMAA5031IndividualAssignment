using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;  // 默认子弹速度
    public float lifeTime = 3f; // 子弹存在时间
    public float power = 1; // 默认子弹威力

    private void Start()
    {
        // 获取 GameManager 中的 bulletSpeed 和 bulletPower，确保同步
        if (GameManager.instance != null)
        {
            speed = GameManager.instance.bulletSpeed ; // 更新子弹速度（乘以合适的倍数）
            power = GameManager.instance.bulletPower; // 更新子弹威力
        }

        Destroy(gameObject, lifeTime); // 避免子弹无限存在
    }

    void Update()
    {
        // 子弹移动
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // 当子弹与敌人碰撞时销毁子弹
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject); // 销毁子弹
        }
    }
}
