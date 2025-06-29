using TMPro;
using UnityEngine;

public class CardCounter : MonoBehaviour
{
    public static CardCounter Instance;
    public GameObject card;

    public TMP_Text counterText;
    public GameObject winPanel;

    private int cardCount = 0;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddSwipe()
    {
        cardCount++;
        UpdateCounter();

        if (cardCount >= 10)
        {
            ShowWinPanel();
        }
    }

    public void ForceWin() 
    {
        ShowWinPanel();
    }

    private void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = $"Ответов: {cardCount}/10";
    }

    private void ShowWinPanel()
{
    if (winPanel != null)
        winPanel.SetActive(true);

    if (card != null)
        card.SetActive(false); 
}


    public void ResetCounter()
    {
        cardCount = 0;
        UpdateCounter();
    }
}
