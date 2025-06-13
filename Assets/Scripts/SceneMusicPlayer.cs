using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneMusicController : MonoBehaviour
{
    public static SceneMusicController Instance;

    [Header("Main Music (для MainMenu, Information, Settings, Choose 1)")]
    public AudioClip mainMusic;
    
    [Header("Garage Music")]
    public AudioClip garageMusic;
    
    [Header("Track 2 Music")]
    public AudioClip track2Music;
    
    [Header("Track 1 Music")]
    public AudioClip track1Music;

    private AudioSource audioSource;
    private string currentSceneName;
    private AudioClip currentMusic;

    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Отключи это временно:
        // RemoveExtraAudioListeners();

        if (GetComponent<AudioListener>() == null)
        {
            gameObject.AddComponent<AudioListener>();
        }
    }
    else
    {
        Destroy(gameObject);
        return;
    }
}


    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckSceneAndPlayMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveExtraAudioListeners(); // Удаляем лишние AudioListener при загрузке сцены
        CheckSceneAndPlayMusic(scene.name);
    }

    void RemoveExtraAudioListeners()
    {
        // Находим все AudioListener в сцене
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        
        // Оставляем только один (на этом объекте)
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
        if (currentSceneName == sceneName) return;
        
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

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}