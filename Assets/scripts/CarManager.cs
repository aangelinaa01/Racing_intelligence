using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CarManager : MonoBehaviour 
{
    public static CarManager Instance;

    [Header("Car Setup")]
    public GameObject[] cars;
    private int currentCarIndex = 0;

    [Header("Car Materials")]
    public Material[] carColors;
    private Renderer carRenderer;

    [Header("Tire Options")]
    public Material[] tireMaterials;
    public MeshRenderer[] tireRenderers;

    [Header("Engine Options")]
    public GameObject[] engineModels;
    public Transform engineMountPoint;
    private GameObject currentEngine;

    [Header("Car Options")]
    public string[] engineNames = { "V6", "V8", "Electric motor" };
    public string[] tireNames = { "Hard", "Medium", "Soft" };
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
    public GameObject loadingPanel;

    [Header("Mount Points")]
    public Transform PointHolder; // Родительский объект с точками для всех машин
    private Transform currentCarPoints; // Точки для текущей машины

    [HideInInspector] public int savedCarIndex;
    [HideInInspector] public int savedColorIndex;
    [HideInInspector] public int savedTireIndex;
    [HideInInspector] public int savedEngineIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadSelection();
        ShowCar(currentCarIndex);
        UpdateUI();
        OpenCarSelectionPanel();
    }

    public void ShowCar(int index)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(i == index);
        }

        currentCarIndex = Mathf.Clamp(index, 0, cars.Length - 1);
        carRenderer = cars[currentCarIndex].GetComponentInChildren<Renderer>();

        currentColorIndex = PlayerPrefs.GetInt($"ColorIndex_{currentCarIndex}", 0);

        ApplyColor();
        ApplyTires();
        ApplyEngine();
        UpdateUI();
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
    private void ApplyColor()
    {
        if (carColors.Length <= currentColorIndex || carRenderer == null) return;

        Color selectedColor = carColors[currentColorIndex].color;
        Material[] mats = carRenderer.materials;

        if (currentCarIndex == 0)
        {
            // Для первой машины меняем два материала (индексы 0 и 3)
            if (mats.Length > 0) mats[0].color = selectedColor;
            if (mats.Length > 3) mats[3].color = selectedColor;
        }
        else
        {
            // Для остальных машин меняем только первый материал
            if (mats.Length > 0) mats[0].color = selectedColor;
        }

        carRenderer.materials = mats;
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

    public void NextCar() => ShowCar((currentCarIndex + 1) % cars.Length);
    public void PrevCar() => ShowCar((currentCarIndex - 1 + cars.Length) % cars.Length);

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
        // Сохраняем цвет для текущей машины
        PlayerPrefs.SetInt($"ColorIndex_{currentCarIndex}", currentColorIndex);
        PlayerPrefs.Save();
    }

    public void OpenTirePanel() => SetPanelActive(tirePanel);
    public void OpenEnginePanel() => SetPanelActive(enginePanel);
    public void OpenColorPanel() => SetPanelActive(colorPanel);
    public void OpenCarSelectionPanel() => SetPanelActive(carSelectionPanel);

    void SetPanelActive(GameObject targetPanel)
    {
        carSelectionPanel.SetActive(false);
        tirePanel.SetActive(false);
        enginePanel.SetActive(false);
        colorPanel.SetActive(false);

        if (targetPanel != null)
            targetPanel.SetActive(true);
    }

    public void SaveCurrentSelection()
    {
        savedCarIndex = currentCarIndex;
        savedColorIndex = currentColorIndex;
        savedTireIndex = currentTireIndex;
        savedEngineIndex = currentEngineIndex;

        PlayerPrefs.SetInt("CarIndex", savedCarIndex);
        PlayerPrefs.SetInt("EngineIndex", savedEngineIndex);
        PlayerPrefs.SetInt("TireIndex", savedTireIndex);
        PlayerPrefs.SetInt($"ColorIndex_{savedCarIndex}", savedColorIndex);
        PlayerPrefs.Save();
    }

    public void CancelSelection()
    {
        // Восстанавливаем сохраненные значения без сохранения
        currentCarIndex = savedCarIndex;
        currentColorIndex = PlayerPrefs.GetInt($"ColorIndex_{currentCarIndex}", 0);
        currentEngineIndex = savedEngineIndex;
        currentTireIndex = savedTireIndex;

        ShowCar(currentCarIndex);
    }

    public void ConfirmAndLoadGameScene()
    {
        SaveCurrentSelection();

        if (loadingPanel != null) loadingPanel.SetActive(true);
        SceneManager.LoadScene("track_2");
    }

    void LoadSelection()
    {
        savedCarIndex = PlayerPrefs.GetInt("CarIndex", 0);
        savedEngineIndex = PlayerPrefs.GetInt("EngineIndex", 0);
        savedTireIndex = PlayerPrefs.GetInt("TireIndex", 0);

        // Загружаем сохраненный цвет для текущей машины
        savedColorIndex = PlayerPrefs.GetInt($"ColorIndex_{savedCarIndex}", 0);

        currentCarIndex = savedCarIndex;
        currentEngineIndex = savedEngineIndex;
        currentTireIndex = savedTireIndex;
        currentColorIndex = savedColorIndex;
    }

    public void ResetSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        ResetSavedData();
    }
}