using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    IsGameOver,
    IsWinner,
    IsPlaying,
    IsPaused,
    InMenu
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text m_BaseLivesText;
    [SerializeField] private Text m_EnemyKillsText;
    [SerializeField] private int m_BaseStartLives = 10;
    [SerializeField] private GameObject m_WinnerScreen;
    [SerializeField] private GameObject m_GameOverScreen;

    private static GameState s_GameState;
    public static GameState GameState { get => s_GameState; set => s_GameState = value; }

    private static int s_TowerHealth;
    private static int s_EnemyKills;
    private static int s_TotalEnemies;

    private static event Action<int> OnHealthChanged;
    private static event Action<int> OnIncreaseKills;
    public static int EnemyKills
    {
        get => s_EnemyKills;
        set
        {
            if (s_EnemyKills != value)
            {
                s_EnemyKills = value;
                OnIncreaseKills?.Invoke(s_EnemyKills);
            }
        }
    }

    public static int TowerHealth
    {
        get => s_TowerHealth;
        set
        {
            if (s_TowerHealth != value)
            {
                s_TowerHealth = value;
                if (s_TowerHealth <= 0)
                {
                    s_TowerHealth = 0;
                    GameState = GameState.IsGameOver;
                }
                OnHealthChanged?.Invoke(s_TowerHealth);
                TotalEnemies--;
            }
        }
    }

    public static int TotalEnemies { get => s_TotalEnemies; set => s_TotalEnemies = value; }

    private void Awake()
    {
        GameState = GameState.IsPlaying;
        s_TowerHealth = m_BaseStartLives;
        s_EnemyKills = 0;

        m_BaseLivesText.text = $"Base Lives: {s_TowerHealth}";
        OnHealthChanged += ChangeHealth;

        m_EnemyKillsText.text = $"Enemy Kills: {EnemyKills}";
        OnIncreaseKills += IncreaseKillCount;

        m_WinnerScreen.SetActive(false);
        m_GameOverScreen.SetActive(false);
    }

    private void ChangeHealth(int amount)
    {
        m_BaseLivesText.text = $"Base Lives: {amount}";

        if (GameState == GameState.IsGameOver)
        {
            m_GameOverScreen.SetActive(true);
        }
    }

    private void IncreaseKillCount(int amount)
    {
        m_EnemyKillsText.text = $"Enemy Kills: {amount}";
        if (s_EnemyKills == s_TotalEnemies)
        {
            GameState = GameState.IsWinner;
            m_WinnerScreen.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        OnHealthChanged -= ChangeHealth;
        OnIncreaseKills -= IncreaseKillCount;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
