using System.Collections;
using UnityEngine;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public GameObject finishPanel;

    private float raceTime;
    private bool raceStarted = false;
    private bool raceFinished = false;
    public bool canFinish = false;

    private int finishLinePasses = 0;
    public int requiredLaps = 1; // Количество кругов (проездов после старта)

    void Start()
    {
         timerText.text = "Время: 0.00";
        if (finishPanel != null)
            finishPanel.SetActive(false);
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "Старт!";
        EnableCarControl();

        raceStarted = true;
        canFinish = true;
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            raceTime += Time.deltaTime;
            timerText.text = "Время: " + raceTime.ToString("F2");
        }
    }

    private void EnableCarControl()
    {
        CarController[] cars = FindObjectsOfType<CarController>();
        foreach (CarController car in cars)
        {
            car.canDrive = true;
        }
    }

    // Вызывается из FinishLine.cs
    public void OnFinishLinePassed()
    {
        if (!canFinish || raceFinished) return;

        finishLinePasses++;
        Debug.Log($"Проезд финиша №{finishLinePasses}");

        if (finishLinePasses > requiredLaps)
        {
            FinishRace();
        }
    }

    private void FinishRace()
{
    raceFinished = true;
    Debug.Log("Финиш! Время: " + raceTime.ToString("F2"));

    if (finishPanel != null)
        finishPanel.SetActive(true);
}
}
