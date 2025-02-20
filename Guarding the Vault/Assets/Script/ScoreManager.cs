using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance; // 单例模式，方便调用
    public int score = 0; // 玩家得分
    public TextMeshPro scoreText; // 3D UI 组件
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score; // 更新 UI 显示
    }
}
