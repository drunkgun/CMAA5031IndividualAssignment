using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
        public float speed = 20f;
            public float lifeTime = 3f; // 子弹存在时间


    // Start is called before the first frame update
    
    void Start()
    {
                Destroy(gameObject, lifeTime); // 避免子弹无限存在

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;

    }
    
        private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Bullet hit: " + other.gameObject.name); // 输出被子弹碰撞的物体名称

    if (other.CompareTag("Enemy"))
    {
                Debug.Log("Bullet hit an Enemy! Destroying..."); // 碰撞到敌人，打印日志

        Destroy(other.gameObject); // 销毁敌人
        Destroy(gameObject); // 销毁子弹

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(10); // 每次击中敌人加10分
        }
    }
    }
}
