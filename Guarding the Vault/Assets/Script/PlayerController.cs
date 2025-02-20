using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public GameObject bulletPrefab; // 子弹预制体
    public Transform bulletSpawn;   // 子弹发射点
    public float bulletSpeed = 20f; // 子弹速度



    // Start is called before the first frame update
    private void Update()
    {
       // 获取输入
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveY = Input.GetAxis("Vertical");   // W/S or Up/Down
        float moveZ = 0; // 可选：添加前后移动

        // 计算移动方向
        Vector3 moveDir = new Vector3(moveX, moveY, moveZ) * moveSpeed * Time.deltaTime;

        // 限制玩家移动范围
        Vector3 newPosition = transform.position + moveDir;
        newPosition.x = Mathf.Clamp(newPosition.x, -10f, 10f);
        newPosition.y = Mathf.Clamp(newPosition.y, -6f, 6f);
        transform.position = newPosition;

        // 监听空格键射击
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        } 
    }
    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawn != null)
        {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.forward * bulletSpeed;
            }
        }
        else
        {
            Debug.LogError("BulletPrefab 或 BulletSpawn 没有正确赋值！");
        }
    }
    // Update is called once per frame

}
