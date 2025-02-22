using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private float nextFireTime = 0f;

    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void Update()
    {
        // 获取输入
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right

        // 计算移动方向
        Vector3 moveDir = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;

        // 限制玩家移动范围
        Vector3 newPosition = transform.position + moveDir;
        newPosition.x = Mathf.Clamp(newPosition.x, -5f, 5f);
        transform.position = newPosition;

        // 自动发射子弹
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + GameManager.instance.fireRate; // 设置下一次发射的时间
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && bulletSpawn != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.forward * GameManager.instance.bulletSpeed;
            }
        }
        else
        {
            Debug.LogError("BulletPrefab 或 BulletSpawn 没有正确赋值！");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.TriggerGameOver();
        }
    }
}