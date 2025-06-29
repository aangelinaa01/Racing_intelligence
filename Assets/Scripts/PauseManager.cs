using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button infoButton;       
    [SerializeField] private Button closeInfoButton;  
    [SerializeField] private Button loadSceneButton; 
    [SerializeField] private string sceneToLoad;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;

        TryAddListener(pauseButton, TogglePause);
        TryAddListener(resumeButton, TogglePause);
        TryAddListener(restartButton, RestartLevel);
        TryAddListener(infoButton, OpenInfoPanel);
        TryAddListener(closeInfoButton, CloseInfoPanel);

        if (loadSceneButton != null)
            loadSceneButton.onClick.AddListener(() => LoadSceneAndClosePanels(sceneToLoad));
        else Debug.LogWarning("loadSceneButton not assigned.");

        if (pausePanel != null) pausePanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void TryAddListener(Button btn, UnityEngine.Events.UnityAction action)
    {
        if (btn != null)
            btn.onClick.AddListener(action);
        else
            Debug.LogWarning($"Button {action.Method.Name} not assigned.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenInfoPanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    public void LoadSceneAndClosePanels(string sceneName)
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }
}
