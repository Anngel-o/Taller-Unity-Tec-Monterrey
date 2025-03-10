using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Win
    }
    public GameState gameState = GameState.Playing;
    public InputActionAsset playerControls;

    [Header("Combo System")]

    public int popCounter;
    public float comboTimeWindow = 1.0f;
    public int popComboTarget = 20;
    public int currentPopCounter = 0;
    float lastPopTime = 0f;
    public float popComboProgress = 0;
    bool mustDecreaseProgress;
    public float decreaseTimerWait = 1.5f;
    float decreaseTimer;
    public float decreaseSpeed = 0.5f;

    void Start()
    {
        Instance = this;
        decreaseTimer = decreaseTimerWait;
        playerControls.Enable();
    }

    public void Pause()
    {
        gameState = GameState.Paused;
        UIManager.Instance.Pause();
    }

    public void Win()
    {
        gameState = GameState.Win;
        UIManager.Instance.Win();
        Debug.Log("¡Has ganado!");
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        Debug.Log("¡Has perdido!");
        UIManager.Instance.GameOver();
    }

    void Update()
    {
        if (mustDecreaseProgress)
        {
            popComboProgress -= Time.deltaTime * decreaseSpeed;
            if (popComboProgress < 0)
            {
                popComboProgress = 0;
            }
        }
        else
        {
            decreaseTimer -= Time.deltaTime;
            if (decreaseTimer <= 0)
            {
                mustDecreaseProgress = true;
            }
        }

        float currentTime = Time.time;
        if (currentTime - lastPopTime > comboTimeWindow)
        {
            currentPopCounter = 0;
        }
        if (currentPopCounter >= popComboTarget)
        {
            Debug.Log("Combo achieved!");
        }

    }

    public void Pop()
    {
        lastPopTime = Time.time;
        popCounter++;
        currentPopCounter++;
        decreaseTimer = decreaseTimerWait;
        popComboProgress += 1.0f / popComboTarget;
        mustDecreaseProgress = false;
    }

    public float ComboCounter()
    {
        return popComboProgress;
    }

    public void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void RestartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
