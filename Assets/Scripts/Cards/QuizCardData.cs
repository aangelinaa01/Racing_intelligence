using UnityEngine;

public enum SwipeDirection { Left, Right }

[System.Serializable]
public class QuizCardData
{
    [TextArea(2, 4)] public string questionText;
    public string leftOption;
    public string rightOption;
    public SwipeDirection correctAnswer;
}
