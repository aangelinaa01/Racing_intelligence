using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pausePanel;  // Панель паузы
    [SerializeField] private GameObject infoPanel;   // Панель информации
    [SerializeField] private Button pauseButton;     // Кнопка паузы
    [SerializeField] private Button resumeButton;    // Кнопка "Продолжить"
    [SerializeField] private Button restartButton;   // Кнопка "Рестарт"
    [SerializeField] private Button infoButton;      // Кнопка для открытия инфо панели
    [SerializeField] private Button closeInfoButton; // Кнопка закрытия инфо панели

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;

        // Назначаем действия кнопкам
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartLevel);

        infoButton.onClick.AddListener(OpenInfoPanel);
        closeInfoButton.onClick.AddListener(CloseInfoPanel);

        // Панели скрыты в начале
        pausePanel.SetActive(false);
        infoPanel.SetActive(false);

        // Курсор всегда активен
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        // Нажатие ESC включает/выключает паузу
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Переключение паузы
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
    }

    // Перезапуск уровня
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Открытие инфо панели
    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    // Закрытие инфо панели
    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
