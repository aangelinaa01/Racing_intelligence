using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject informationPanel;

    private void Awake()
    {
        if (settingsPanel == null)
            settingsPanel = GameObject.Find("SettingsPanel");

        if (informationPanel == null)
            informationPanel = GameObject.Find("InformationPanel");

        if (settingsPanel == null) Debug.LogWarning("SettingsPanel not found in scene.");
        if (informationPanel == null) Debug.LogWarning("InformationPanel not found in scene.");
    }

    private void Start()
    {
        Debug.Log("GameStart");
        ClosePanels();
    }

    public void ShowSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (informationPanel != null) informationPanel.SetActive(false);
    }

    public void ShowInformation()
    {
        if (informationPanel != null) informationPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void ClosePanels()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (informationPanel != null) informationPanel.SetActive(false);
    }

    public void GoToGarage() => SceneManager.LoadScene("Garage 1");
    public void GoToTrack1() => SceneManager.LoadScene("track_1");
    public void GoToTrack2() => SceneManager.LoadScene("track_2");
    public void GoToChoose1() => SceneManager.LoadScene("Choose 1");
    public void GoToChoose2() => SceneManager.LoadScene("Choose 2");

    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();
    }

    public void GoBack()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = Mathf.Max(currentSceneIndex - 1, 0);
        SceneManager.LoadScene(previousSceneIndex);
    }
}
