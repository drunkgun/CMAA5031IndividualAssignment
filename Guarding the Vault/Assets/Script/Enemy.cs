using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;

    private void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z < -10) // 超出视野销毁
        {
            Destroy(gameObject);
        }
    }


   private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Enemy hit the Player!");

        // 立即销毁敌人
        Destroy(gameObject); 
        

        // 减少分数
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(-5); // 减分
        }
    }
}


}
