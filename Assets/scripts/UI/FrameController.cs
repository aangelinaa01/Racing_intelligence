using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FrameController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Button applyButton;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;

    private Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1280, 720),
        new Vector2Int(2560, 1440),
        new Vector2Int(3840, 2160)
    };
    
    private void Start()
    {
        
        int savedResolution = PlayerPrefs.GetInt("ResolutionIndex", 0);
        int savedQuality = PlayerPrefs.GetInt("QualityIndex", 2);
        
        
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f); // 0.5 = 50%

        resolutionDropdown.value = savedResolution;
        qualityDropdown.value = savedQuality;
        volumeSlider.value = savedVolume;
        volumeText.text = Mathf.RoundToInt(savedVolume * 100f) + "%";

        AudioListener.volume = savedVolume;

        ApplySettings();

        applyButton.onClick.AddListener(ApplySettings);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ApplySettings()
    {
       
        int resIndex = resolutionDropdown.value;
        if (resIndex >= 0 && resIndex < resolutions.Length)
        {
            Vector2Int selectedResolution = resolutions[resIndex];
            Screen.SetResolution(selectedResolution.x, selectedResolution.y, true);
        }

       
        int selectedDropdownQuality = qualityDropdown.value;

        
        int[] mappedQualityLevels = new int[] { 0, 1, 2 };

        if (selectedDropdownQuality >= 0 && selectedDropdownQuality < mappedQualityLevels.Length)
        {
            int unityQualityIndex = mappedQualityLevels[selectedDropdownQuality];
            if (unityQualityIndex < QualitySettings.names.Length)
            {
                QualitySettings.SetQualityLevel(unityQualityIndex);
                Debug.Log($"Графика установлена на: {QualitySettings.names[unityQualityIndex]}");
            }
            else
            {
                Debug.LogWarning("Недопустимый уровень качества!");
            }
        }

        PlayerPrefs.SetInt("ResolutionIndex", resIndex);
        PlayerPrefs.SetInt("QualityIndex", selectedDropdownQuality);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();

       
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
        volumeText.text = Mathf.RoundToInt(value * 100f) + "%";
        
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}