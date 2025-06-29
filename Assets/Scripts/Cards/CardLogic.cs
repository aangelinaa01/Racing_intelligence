using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CardLogic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TMP_Text questionText;
    public TMP_Text leftOptionText;
    public TMP_Text rightOptionText;

    private Vector2 startPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public float swipeThreshold = 150f;
    public float rotationMultiplier = 0.2f;
    public float swipeDuration = 0.3f;
    public float returnDuration = 0.2f;
    

    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    

    public void ShowCard(QuizCardData data)
    {
        if (data == null) return;
        questionText.text = data.questionText;
        leftOptionText.text = data.leftOption;
        rightOptionText.text = data.rightOption;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.delta;
        rectTransform.anchoredPosition += dragDelta;

        float rotationZ = (rectTransform.anchoredPosition.x - startPosition.x) * rotationMultiplier;
        rectTransform.rotation = Quaternion.Euler(0, 0, -rotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float displacement = rectTransform.anchoredPosition.x - startPosition.x;

        if (displacement < -swipeThreshold)
        {
            // Свайп влево
            StartCoroutine(SwipeOut(Vector2.left * 1000, SwipeDirection.Left));
        }
        else if (displacement > swipeThreshold)
        {
            // Свайп вправо
            StartCoroutine(SwipeOut(Vector2.right * 1000, SwipeDirection.Right));
        }
        else
        {
            // Недостаточный свайп — вернуть обратно
            StartCoroutine(ReturnToStart());
        }

        canvasGroup.blocksRaycasts = true;
    }

   private IEnumerator SwipeOut(Vector2 direction, SwipeDirection swipeDir)
{
    Vector2 start = rectTransform.anchoredPosition;
    Vector2 target = start + direction.normalized * 700f; 
    float elapsed = 0f;

    Quaternion startRot = rectTransform.rotation;
    Quaternion targetRot = Quaternion.Euler(0, 0, -Mathf.Sign(direction.x) * 25f); 

    while (elapsed < swipeDuration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / swipeDuration;

        
        float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);

        rectTransform.anchoredPosition = Vector2.Lerp(start, target, easedT);
        rectTransform.rotation = Quaternion.Lerp(startRot, targetRot, easedT);

        yield return null;
    }

    QuizManager.Instance.SubmitAnswer(swipeDir);
    UpdateCard();
}


    private IEnumerator ReturnToStart()
    {
        Vector2 current = rectTransform.anchoredPosition;
        Quaternion currentRot = rectTransform.rotation;

        float elapsed = 0f;
        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDuration;

            rectTransform.anchoredPosition = Vector2.Lerp(current, startPosition, t);
            rectTransform.rotation = Quaternion.Lerp(currentRot, Quaternion.identity, t);

            yield return null;
        }

        rectTransform.anchoredPosition = startPosition;
        rectTransform.rotation = Quaternion.identity;
    }

    private void UpdateCard()
    {
        rectTransform.rotation = Quaternion.identity;
        var nextCard = QuizManager.Instance.GetCurrentCard();
        if (nextCard != null)
        {
            ShowCard(nextCard);
            rectTransform.anchoredPosition = startPosition;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
