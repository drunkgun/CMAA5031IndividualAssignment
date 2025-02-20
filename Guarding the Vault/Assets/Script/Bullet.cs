using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f; // 子弹存在时间
    public int power = 1; // 子弹威力

    void Start()
    {
        Destroy(gameObject, lifeTime); // 避免子弹无限存在
    }

    void Update()
    {
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
