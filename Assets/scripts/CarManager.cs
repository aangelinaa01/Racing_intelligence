using UnityEngine;
using TMPro;

public class CarManager : MonoBehaviour
{
    [Header("Car Setup")]
    public GameObject[] cars;
    private int currentCarIndex = 0;

    [Header("Car Materials")]
    public Material[] carColors;
    private Renderer carRenderer;

    [Header("Tire Options")]
    public Material[] tireMaterials; // Материалы для шин
    public MeshRenderer[] tireRenderers; // Рендереры всех шин на машине

    [Header("Engine Options")]
    public GameObject[] engineModels; // 3D модели двигателей
    public Transform engineMountPoint; // Точка крепления двигателя на машине
    private GameObject currentEngine;

    [Header("Car Options")]
    public string[] engineNames = { "V6", "V8", "Electric" };
    public string[] tireNames = { "Sport", "Offroad", "Street" };
    private int currentEngineIndex = 0;
    private int currentTireIndex = 0;
    private int currentColorIndex = 0;

    [Header("UI Texts")]
    public TextMeshProUGUI carNameText;
    public TextMeshProUGUI engineNameText;
    public TextMeshProUGUI tireNameText;

    [Header("UI Panels")]
    public GameObject carSelectionPanel;
    public GameObject tirePanel;
    public GameObject enginePanel;
    public GameObject colorPanel;

    void Start()
    {
        
        ResetSavedData();
        
        LoadSelection();
        ShowCar(currentCarIndex);
        UpdateUI();
        OpenCarSelectionPanel();
    }

    void OnApplicationQuit()
    {
        // Сброс сохранений при закрытии игры
        ResetSavedData();
    }

    public void ShowCar(int index)
{
    for (int i = 0; i < cars.Length; i++)
    {
        cars[i].SetActive(i == index);
    }

    currentCarIndex = Mathf.Clamp(index, 0, cars.Length - 1);
    GameObject currentCar = cars[currentCarIndex];
    carRenderer = currentCar.GetComponentInChildren<Renderer>();

    string colorKey = "ColorIndex_" + currentCarIndex;

    if (PlayerPrefs.HasKey(colorKey))
    {
        currentColorIndex = PlayerPrefs.GetInt(colorKey);
        ApplyColor();
    }
    
    ApplyTires();
    ApplyEngine();
    UpdateUI();
}


    public void NextCar()
    {
        ShowCar((currentCarIndex + 1) % cars.Length);
    }

    public void PrevCar()
    {
        ShowCar((currentCarIndex - 1 + cars.Length) % cars.Length);
    }

    public void NextEngine()
    {
        currentEngineIndex = (currentEngineIndex + 1) % engineNames.Length;
        ApplyEngine();
        UpdateUI();
    }

    public void PrevEngine()
    {
        currentEngineIndex = (currentEngineIndex - 1 + engineNames.Length) % engineNames.Length;
        ApplyEngine();
        UpdateUI();
    }

    public void NextTire()
    {
        currentTireIndex = (currentTireIndex + 1) % tireNames.Length;
        ApplyTires();
        UpdateUI();
    }

    public void PrevTire()
    {
        currentTireIndex = (currentTireIndex - 1 + tireNames.Length) % tireNames.Length;
        ApplyTires();
        UpdateUI();
    }

    public void SetColor(int index)
{
    currentColorIndex = index;
    ApplyColor();
    PlayerPrefs.SetInt("ColorIndex_" + currentCarIndex, currentColorIndex); // сохраняем выбор
    PlayerPrefs.Save();
    UpdateUI();
}


    private void ApplyColor()
{
    if (carRenderer != null && carColors.Length > currentColorIndex)
    {
        carRenderer.material = carColors[currentColorIndex];
    }
}


    
    private void ApplyTires()
    {
        if (tireRenderers != null && tireMaterials.Length > currentTireIndex)
        {
            foreach (var renderer in tireRenderers)
            {
                Material[] mats = renderer.materials;

                // Убедимся, что у объекта есть второй материал
                if (mats.Length > 1)
                {
                    mats[3] = tireMaterials[currentTireIndex];
                    renderer.materials = mats;
                }
                else
                {
                    Debug.LogWarning($"{renderer.name} не имеет второго элемента материала (Element 1)");
                }
            }
        }
    }

    private void ApplyEngine()
    {
        // Удаляем старый двигатель
        if (currentEngine != null)
        {
            Destroy(currentEngine);
        }

        // Создаем новый двигатель
        if (engineModels.Length > currentEngineIndex && engineMountPoint != null)
        {
            currentEngine = Instantiate(engineModels[currentEngineIndex], engineMountPoint);
            currentEngine.transform.localPosition = Vector3.zero;
            currentEngine.transform.localRotation = Quaternion.identity;
        }
    }

    private void UpdateUI()
{
    if (carNameText != null)
        carNameText.text = cars[currentCarIndex].name;

    if (engineNameText != null)
        engineNameText.text = "Двигатель: " + engineNames[currentEngineIndex];

    if (tireNameText != null)
        tireNameText.text = "Шины: " + tireNames[currentTireIndex];
}


    public void OpenTirePanel()
    {
        SetPanelActive(tirePanel);
    }

    public void OpenEnginePanel()
    {
        SetPanelActive(enginePanel);
    }

    public void OpenColorPanel()
    {
        SetPanelActive(colorPanel);
    }

   

    public void OpenCarSelectionPanel()
    {
        SetPanelActive(carSelectionPanel);
    }

    public void ConfirmSelection()
    {
        SaveSelection();
    }

    void SetPanelActive(GameObject targetPanel)
    {
        carSelectionPanel.SetActive(false);
        tirePanel.SetActive(false);
        enginePanel.SetActive(false);
        colorPanel.SetActive(false);

        if (targetPanel != null)
            targetPanel.SetActive(true);
    }

    void SaveSelection()
    {
        PlayerPrefs.SetInt("CarIndex", currentCarIndex);
        PlayerPrefs.SetInt("EngineIndex", currentEngineIndex);
        PlayerPrefs.SetInt("TireIndex", currentTireIndex);
        PlayerPrefs.SetInt("ColorIndex", currentColorIndex);
        PlayerPrefs.Save();
    }

    void LoadSelection()
    {
        currentCarIndex = PlayerPrefs.GetInt("CarIndex", 0);
        currentEngineIndex = PlayerPrefs.GetInt("EngineIndex", 0);
        currentTireIndex = PlayerPrefs.GetInt("TireIndex", 0);
        currentColorIndex = PlayerPrefs.GetInt("ColorIndex", 0);
    }

    void ResetSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}