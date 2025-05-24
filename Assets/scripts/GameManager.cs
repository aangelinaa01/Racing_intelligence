using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("GameStart");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoToInformation()
    {
        SceneManager.LoadScene("Information");
    }

    public void GoToGarage()
    {
        SceneManager.LoadScene("Garage 1");
    }

    public void GoToTrack1()
    {
        SceneManager.LoadScene("track_1");
    }

    public void GoToTrack2()
    {
        SceneManager.LoadScene("track_2");
    }

    public void GoToChoose1()
    {
        SceneManager.LoadScene("Choose 1");
    }

    public void GoToChoose2()
    {
        SceneManager.LoadScene("Choose 2");
    }
    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();
    }

}
