using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public CardLogic cl;
    public RectTransform card;

    private Vector2 initialCardPos;
    // public static GameLogic Instance;

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject); 
    //     }
    //     else
    //     {
    //         Destroy(gameObject); 
    //     }
    // }
    void Start()
    {
        if (card != null)
            initialCardPos = card.anchoredPosition;
        cl.ShowCard(QuizManager.Instance.GetCurrentCard());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            QuizManager.Instance.SubmitAnswer(SwipeDirection.Left);
            UpdateCard();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            QuizManager.Instance.SubmitAnswer(SwipeDirection.Right);
            UpdateCard();
        }
    }

    private void UpdateCard()
    {
        var nextCard = QuizManager.Instance.GetCurrentCard();
        if (nextCard != null)
        {
            cl.ShowCard(nextCard);
            ResetCardPosition();
        }
        else
        {
            card.gameObject.SetActive(false);
        }
    }

    private void ResetCardPosition()
    {
        if (card != null)
            card.anchoredPosition = initialCardPos;
    }
}
