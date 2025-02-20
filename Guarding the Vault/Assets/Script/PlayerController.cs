using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 引入UI命名空间

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab; // 子弹预制体
    public Transform bulletSpawn;   // 子弹发射点
    public float bulletSpeed = 25f; // 子弹速度

    public int bulletPower = 1; // 子弹威力

    public Button increaseSpeedButton;  // 提升速度按钮
    public Button increasePowerButton; // 提升威力按钮

    public GameObject skillSelectionMenu; // 技能选择菜单
    private int playerLevel = 1; // 玩家等级
    private int roundsCompleted = 0; // 完成的轮次

    // 自动发射相关变量
    public float fireRate = 0.5f; // 子弹发射间隔（秒）
    private float nextFireTime = 0f; // 下一次发射时间

    private void Start()
    {
        // 为按钮添加点击事件
        if (increaseSpeedButton != null)
        {
            increaseSpeedButton.onClick.AddListener(IncreaseBulletSpeed);  // 增加子弹速度
        }

        if (increasePowerButton != null)
        {
            increasePowerButton.onClick.AddListener(IncreaseBulletPower);  // 提升子弹威力
        }

        // 隐藏技能选择菜单
        skillSelectionMenu.SetActive(false);
    }

    // Update is called once per frame
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
            nextFireTime = Time.time + fireRate;  // 设置下一次发射的时间
        }

        // 检查是否可以升级并显示技能选择菜单
        if (roundsCompleted >= 3 * playerLevel) // 每3轮升级一次
        {
            ShowSkillSelection();
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

    // 玩家技能选择界面显示
    public void ShowSkillSelection()
    {
        // 显示技能选择菜单
        skillSelectionMenu.SetActive(true); // 显示技能选择菜单
        Time.timeScale = 0; // 暂停游戏时间
        Debug.Log("技能选择界面显示，游戏暂停");
    }

    // 隐藏技能选择界面并恢复时间
    public void HideSkillSelection()
    {
        // 隐藏技能选择菜单
        skillSelectionMenu.SetActive(false); // 隐藏技能选择菜单
        Time.timeScale = 1; // 恢复游戏时间
        playerLevel++; // 升级玩家等级
        Debug.Log("技能选择界面隐藏，游戏继续，玩家升级到等级 " + playerLevel);
    }

    // 增加子弹速度
    public void IncreaseBulletSpeed()
    {
        bulletSpeed *= 1.5f; // 增加子弹速度
        Debug.Log("子弹速度增加！当前速度: " + bulletSpeed);
        HideSkillSelection(); // 选择后隐藏菜单并恢复时间
    }

    // 提升子弹威力
    public void IncreaseBulletPower()
    {
        bulletPower = (int)(bulletPower * 1.3f); // 提升子弹威力
        Debug.Log("子弹威力增加！当前威力: " + bulletPower);
        HideSkillSelection(); // 选择后隐藏菜单并恢复时间
    }

    // 玩家完成一轮
    public void CompleteRound()
    {
        roundsCompleted++;
    }
}
