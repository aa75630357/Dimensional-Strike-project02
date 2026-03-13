using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI; // 記得引入 UI 字典
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("UI控制")]
    public Image hp;
    public Image ammo;
    public TextMeshProUGUI scoreText;
    public GameObject crosshairUI;      //準星
    [Header("數值")]
    public float maxhp = 100;
    public float currentHP;
    public int score = 0;
    [Header("結束書面控制")]
    public GameObject EndGamePanel;
    public TextMeshProUGUI FinalScoreText;
    public bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        if (EndGamePanel != null) EndGamePanel.SetActive(false);
        currentHP = maxhp;
        UpdateHP();
        scoreText.text = "Score: " + score;
        if (crosshairUI != null) crosshairUI.SetActive(true); // 確保一開始準星有顯示
    }
    void Update()
    {
        if (isGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void AddScore(int point)             //抓子彈code的加分
    {
        score += point;
        scoreText.text = "Score: " + score;
    }
    public void TakeDamage(float Damage)        //抓怪物AI的code來判斷怪物是否有撞到玩家(之後可能從怪物子彈抓)
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            GameOver();
            //呼叫endGameUI與關掉其他東西
        }
        UpdateHP();

    }
    public void UpdateAmmo(float fillAmount)//抓彈藥的
    {
        if (ammo != null)
        {
            ammo.fillAmount = fillAmount;
        }
    }
    void UpdateHP()
    {
        if (hp != null)
        {
            hp.fillAmount = currentHP / maxhp;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        if (EndGamePanel != null) EndGamePanel.SetActive(true);
        if (FinalScoreText != null) FinalScoreText.text = "Final Score: " + score;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void RestartGame()
    {// 1. 解除時間暫停 (超級重要！不然重開後遊戲還是當機狀態)
        Time.timeScale = 1f;

        // 2. 讀取「現在正在玩的這個場景」，達成完美重置！
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
