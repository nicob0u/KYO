using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.InputSystem;
using System.Diagnostics;
using DG.Tweening;



public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Win,
    Settings
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public GameState CurrentState;
    public UIManager uiManager;
    private GameState previousState;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeAfterSceneLoad(scene));

    }

    private IEnumerator InitializeAfterSceneLoad(Scene scene)
    {
        yield return null;

        uiManager = FindObjectOfType<UIManager>();

        if (scene.buildIndex == 0)
        {
            SetGameState(GameState.MainMenu);
        }
        else
        {
            SetGameState(GameState.Playing);
        }
    }

    void Update()
    {
        if ((CurrentState == GameState.Playing || CurrentState == GameState.Paused) && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PauseOrResume();
        }
    }

    public void SetGameState(GameState newState)
    {
        if (newState == GameState.Settings)
        {
            previousState = CurrentState;
        }

        CurrentState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                uiManager?.ShowMainMenu();
                uiManager?.HideHUD();
                uiManager?.ShowLogo();
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                uiManager?.HideAllMenus();
                uiManager?.ShowHUD();
                uiManager?.HideLogo();
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                uiManager?.ShowPauseMenu();
                uiManager?.HideHUD();
                uiManager?.HideLogo();
                break;

            case GameState.Win:
                Time.timeScale = 0f;
                uiManager?.ShowWinMenu();
                uiManager?.HideHUD();
                uiManager?.HideLogo();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                uiManager?.ShowGameOverMenu();
                uiManager?.HideHUD();
                uiManager?.HideLogo();
                break;

            case GameState.Settings:
                Time.timeScale = 0f;
                uiManager?.ShowSettings();
                uiManager?.HideHUD();
                uiManager?.HideLogo();
                break;


        }

    }

    public void PauseOrResume()
    {
        if (CurrentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
            UnityEngine.Debug.Log("Resume button clicked");
            Time.timeScale = 1f;
        }
        else if (CurrentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);

        }

    }

    public void LoadLevel(int index)
    {

        index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index <= 4)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            UnityEngine.Debug.Log("No more levels.");
        }


    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UnityEngine.Debug.Log("Restart button pressed.");
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }
    public void Win()
    {
        DOTween.KillAll();
        SetGameState(GameState.Win);
    }

    public void MainMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SetGameState(GameState.MainMenu);
        }

        UnityEngine.Debug.Log("Main menu button pressed.");


    }


    public void Settings()
    {
        SetGameState(GameState.Settings);
        UnityEngine.Debug.Log("Settings button pressed.");

    }

    public void Back()
    {

        if (CurrentState == GameState.Settings)
        {
            uiManager?.HideAllMenus();
            SetGameState(previousState);
            UnityEngine.Debug.Log("Back button pressed");

        }
        else
        {
            UnityEngine.Debug.LogWarning($"Request invalid. Game state: {GameManager.Instance.CurrentState}");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void ExitGame()
    {

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }


}
