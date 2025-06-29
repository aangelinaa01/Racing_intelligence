using UnityEngine;
using UnityEngine.UI;

public class TestPanelController : MonoBehaviour
{
    [Header("Основные панели")]
    public GameObject testPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Кнопки управления")]
    public Button openTestButton1; 
    public Button openTestButton2; 
    public Button closeTestButton;

    void Start()
    {
        if (openTestButton1 != null)
            openTestButton1.onClick.AddListener(ShowTestPanel);
        else Debug.LogWarning("openTestButton1 not assigned.");

        if (openTestButton2 != null)
            openTestButton2.onClick.AddListener(ShowTestPanel);
        else Debug.LogWarning("openTestButton2 not assigned.");

        if (closeTestButton != null)
            closeTestButton.onClick.AddListener(HideTestPanel);
        else Debug.LogWarning("closeTestButton not assigned.");

        HideAllPanels();
    }

    public void ShowTestPanel()
    {
        HideAllPanels();
        if (testPanel != null) testPanel.SetActive(true);

        if (CardCounter.Instance != null)
            CardCounter.Instance.ResetCounter();

        if (QuizManager.Instance != null)
            QuizManager.Instance.ResetQuiz();
    }

    public void HideTestPanel()
    {
        if (testPanel != null) testPanel.SetActive(false);
    }

    public void ShowWinPanel()
    {
        HideAllPanels();
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void ShowLosePanel()
    {
        HideAllPanels();
        if (losePanel != null) losePanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        if (testPanel != null) testPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }
}
