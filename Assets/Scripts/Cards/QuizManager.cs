using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [Tooltip("Добавь сюда все карточки с вопросами и ответами")]
    public List<QuizCardData> quizCards;

    [Tooltip("Порог для победы — сколько нужно правильных ответов")]
    public int correctAnswersThreshold = 7;

    private int currentCardIndex = 0;
    private int correctAnswers = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public QuizCardData GetCurrentCard()
    {
        if (currentCardIndex < quizCards.Count)
            return quizCards[currentCardIndex];
        return null;
    }

    public void SubmitAnswer(SwipeDirection swipe)
{
    var card = GetCurrentCard();
    if (card == null) return;

    if (swipe == card.correctAnswer)
        correctAnswers++;

    currentCardIndex++;
    CardCounter.Instance.AddSwipe();

    if (currentCardIndex >= quizCards.Count)
    {
        if (correctAnswers >= correctAnswersThreshold)
        {
            CardCounter.Instance.ForceWin(); // вызывает ShowWinPanel()
        }
        else
        {
            Debug.Log("Мало правильных ответов. Попробуй снова.");

            // ВАЖНО: вызвать проигрыш
            var panelController = FindObjectOfType<TestPanelController>();
            if (panelController != null)
                panelController.ShowLosePanel();
        }
    }
}


    public void ResetQuiz()
    {
        currentCardIndex = 0;
        correctAnswers = 0;
    }
}
