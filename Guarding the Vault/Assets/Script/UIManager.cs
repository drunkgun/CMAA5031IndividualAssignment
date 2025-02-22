using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject resultUI;
    [SerializeField] private GameObject skillUI;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text successText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button bulletPowerButton;
    [SerializeField] private Button fireRateButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HideAllUI()
    {
        resultUI.SetActive(false);
        skillUI.SetActive(false);
    }

    public void ShowSkillButtons()
    {
        skillUI.SetActive(true);
    }

    public void HideSkillButtons()
    {
        skillUI.SetActive(false);
    }

    public void ShowResultUI(bool isGameOver)
    {
        resultUI.SetActive(true);
        gameOverText.gameObject.SetActive(isGameOver);
        successText.gameObject.SetActive(!isGameOver);
    }

// UIManager.cs
public void BindSkillButtons(System.Action onBulletPowerUpgrade, System.Action onFireRateUpgrade)
{
    bulletPowerButton.onClick.RemoveAllListeners();
    bulletPowerButton.onClick.AddListener(() => onBulletPowerUpgrade());

    fireRateButton.onClick.RemoveAllListeners();
    fireRateButton.onClick.AddListener(() => onFireRateUpgrade());
}

    public void BindPlayAgainButton(System.Action onPlayAgain)
    {
        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(() => onPlayAgain());
    }
}