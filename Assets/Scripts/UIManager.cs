using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    private GameObject mainMenuPanel;
    private GameObject pauseMenuPanel;
    private GameObject gameOverPanel;
    private GameObject winPanel;
    private GameObject hudPanel;
    private GameObject settingsPanel;

    private Button resumeButton;
    private Button pauseRestartButton;
    private Button gameOverRestartButton;
    private Button mainMenuSettingsButton;
    private Button pauseSettingsButton;
    private Button startButton;
    private Button exitButton;
    private Button backButton;
    private Button nextLevelButton;
    private Button winMainMenuButton;
    private Button pauseMainMenuButton;
    private Button gameOverMainMenuButton;

    void Awake()
    {
      
        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        AssignUIReferences();
    }


    void AssignUIReferences()
    {
        mainMenuPanel = GameObject.Find("MainMenuPanel");
        pauseMenuPanel = GameObject.Find("PauseMenuPanel");
        gameOverPanel = GameObject.Find("GameOverPanel");
        winPanel = GameObject.Find("WinPanel");
        hudPanel = GameObject.Find("HUDPanel");
        settingsPanel = GameObject.Find("SettingsPanel");

        if (pauseMenuPanel != null)
        {
            resumeButton = pauseMenuPanel.transform.Find("ResumeButton")?.GetComponent<Button>();
            pauseRestartButton = pauseMenuPanel.transform.Find("RestartButton")?.GetComponent<Button>();
            pauseSettingsButton = pauseMenuPanel.transform.Find("SettingsButton")?.GetComponent<Button>();
            pauseMainMenuButton = pauseMenuPanel.transform.Find("MainMenuButton")?.GetComponent<Button>();
        }

        if (mainMenuPanel != null)
        {
            startButton = mainMenuPanel.transform.Find("StartButton")?.GetComponent<Button>();
            mainMenuSettingsButton = mainMenuPanel.transform.Find("SettingsButton")?.GetComponent<Button>();
            exitButton = mainMenuPanel.transform.Find("ExitButton")?.GetComponent<Button>();

        }
        if (settingsPanel != null)
        {
            backButton = settingsPanel.transform.Find("BackButton")?.GetComponent<Button>();

        }
        if (winPanel != null)
        {
            nextLevelButton = winPanel.transform.Find("NextLevelButton")?.GetComponent<Button>();
            winMainMenuButton = winPanel.transform.Find("MainMenuButton")?.GetComponent<Button>();

        }
        if(gameOverPanel != null)
        {
            gameOverRestartButton = gameOverPanel.transform.Find("RestartButton")?.GetComponent<Button>();
            gameOverMainMenuButton = gameOverPanel.transform.Find("MainMenuButton")?.GetComponent<Button>();
        }

        SetupButtonListeners();
    }



    public void SetupButtonListeners()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(() => GameManager.Instance.PauseOrResume());
        }
        if (pauseRestartButton != null)
        {
            pauseRestartButton.onClick.RemoveAllListeners();
            pauseRestartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());
        }

        if (gameOverRestartButton != null)
        {
            gameOverRestartButton.onClick.RemoveAllListeners();
            gameOverRestartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());
        }

        if (pauseSettingsButton != null)
        {
            pauseSettingsButton.onClick.RemoveAllListeners();
            pauseSettingsButton.onClick.AddListener(() => GameManager.Instance.Settings());
        }
        if (mainMenuSettingsButton != null)
        {
            mainMenuSettingsButton.onClick.RemoveAllListeners();
            mainMenuSettingsButton.onClick.AddListener(() => GameManager.Instance.Settings());
        }

        if (pauseMainMenuButton != null)
        {
            pauseMainMenuButton.onClick.RemoveAllListeners();
            pauseMainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu());
        }
        if (gameOverMainMenuButton != null)
        {
            gameOverMainMenuButton.onClick.RemoveAllListeners();
            gameOverMainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu());
        }
        if (winMainMenuButton != null)
        {
            winMainMenuButton.onClick.RemoveAllListeners();
            winMainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu());
        }
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() => GameManager.Instance.LoadLevel(0));
        }
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(() => GameManager.Instance.ExitGame());
        }
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => GameManager.Instance.Back());
        }
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveAllListeners();
            //nextLevelButton.onClick.AddListener(() => GameManager.Instance.LoadLevel());
        }
    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }


    public void ShowPauseMenu()
    {

        HideAllMenus();
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ShowWinMenu()
    {

        HideAllMenus();
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void ShowGameOverMenu()
    {

        HideAllMenus();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void HideAllMenus()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

    }

    public void ShowSettings()
    {
        HideAllMenus();
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }


    public void ShowHUD()
    {
       if (hudPanel != null) hudPanel.SetActive(true);
    }

    public void HideHUD()
    {
        if (hudPanel != null) hudPanel.SetActive(false);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
