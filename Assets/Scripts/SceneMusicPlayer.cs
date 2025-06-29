using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

[RequireComponent(typeof(AudioSource))]
public class SceneMusicController : MonoBehaviour
{
    public static SceneMusicController Instance;

    [Header("Main Music (MainMenu, Information, Settings, Choose 1, Tests)")]
    public AudioClip mainMusic;

    [Header("Garage Music")]
    public AudioClip garageMusic;

    [Header("Track 1 Music")]
    public AudioClip track1Music;

    [Header("Track 2 Music")]
    public AudioClip track2Music;

    private AudioSource audioSource;
    private string currentSceneName;
    private AudioClip currentMusic;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (GetComponent<AudioListener>() == null)
        {
            gameObject.AddComponent<AudioListener>();
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckSceneAndPlayMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveExtraAudioListeners();
        CheckSceneAndPlayMusic(scene.name);
    }

    void RemoveExtraAudioListeners()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        foreach (AudioListener listener in listeners)
        {
            if (listener.gameObject != gameObject)
            {
                Destroy(listener);
            }
        }
    }

    void CheckSceneAndPlayMusic(string sceneName)
    {
        if (currentSceneName == sceneName)
        {
            
            if (!audioSource.isPlaying && currentMusic != null)
            {
                audioSource.clip = currentMusic;
                audioSource.Play();
            }
            return;
        }

        currentSceneName = sceneName;
        AudioClip newMusic = GetMusicForScene(sceneName);

        if (newMusic != currentMusic)
        {
            currentMusic = newMusic;

            if (newMusic != null)
            {
                audioSource.clip = newMusic;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    AudioClip GetMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
            case "Information":
            case "Settings":
            case "Choose 1":
            case "Tests":
                return mainMusic;

            case "Garage 1":
                return garageMusic;

            case "track_1":
                return track1Music;

            case "track_2":
                return track2Music;

            default:
                return null;
        }
    }

    IEnumerator Crossfade(AudioClip newClip)
    {
        float duration = 1f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = 1 - t / duration;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = t / duration;
            yield return null;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
