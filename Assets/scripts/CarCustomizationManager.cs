using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CarCustomizationManager : MonoBehaviour
{
    public static CarCustomizationManager Instance;

    [Header("Car Setup")]
    public GameObject[] cars;
    public Material[] carColors;
    public Material[] tireMaterials;
    public GameObject[] engineModels;

    [Header("Mount Points (For Final Scene)")]
    public Transform carSpawnPoint; // Для итоговой сцены
    public Camera sceneCamera;

    [Header("UI")]
    public TextMeshProUGUI carNameText, engineNameText, tireNameText;
    public GameObject carPanel, enginePanel, tirePanel, colorPanel;

    [Header("Other")]
    public MeshRenderer[] tireRenderers;
    public Transform engineMountPoint;
    
    // Индексы выбора
    private int carIndex = 0;
    private int colorIndex = 0;
    private int tireIndex = 0;
    private int engineIndex = 0;

    private Renderer carRenderer;
    private GameObject currentEngine;
    private GameObject currentCar;

    // Данные, сохраняемые между сценами
    private int savedCarIndex, savedColorIndex, savedTireIndex, savedEngineIndex;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // В финальной сцене загружаем машину
        if (SceneManager.GetActiveScene().name == "track_1")
        {
            LoadFinalCar();
        }
    }

    private void Start()
    {
        if (cars.Length > 0)
        {
            ShowCar(carIndex);
            UpdateUI();
        }
    }

    // ---------- Кастомизация ----------
    public void NextCar() { ShowCar((carIndex + 1) % cars.Length); }
    public void PrevCar() { ShowCar((carIndex - 1 + cars.Length) % cars.Length); }

    public void NextEngine() { engineIndex = (engineIndex + 1) % engineModels.Length; ApplyEngine(); UpdateUI(); }
    public void PrevEngine() { engineIndex = (engineIndex - 1 + engineModels.Length) % engineModels.Length; ApplyEngine(); UpdateUI(); }

    public void NextTire() { tireIndex = (tireIndex + 1) % tireMaterials.Length; ApplyTires(); UpdateUI(); }
    public void PrevTire() { tireIndex = (tireIndex - 1 + tireMaterials.Length) % tireMaterials.Length; ApplyTires(); UpdateUI(); }

    public void SetColor(int index)
    {
        colorIndex = index;
        ApplyColor();
    }

    void ShowCar(int index)
    {
        for (int i = 0; i < cars.Length; i++)
            cars[i].SetActive(i == index);

        carIndex = index;
        currentCar = cars[carIndex];
        carRenderer = currentCar.GetComponentInChildren<Renderer>();

        ApplyColor();
        ApplyTires();
        ApplyEngine();
        UpdateUI();
    }

    void ApplyColor()
    {
        if (carRenderer != null && colorIndex < carColors.Length)
            carRenderer.material = carColors[colorIndex];
    }

    void ApplyTires()
    {
        foreach (var r in tireRenderers)
        {
            var mats = r.materials;
            if (mats.Length > 1)
            {
                mats[3] = tireMaterials[tireIndex];
                r.materials = mats;
            }
        }
    }

    void ApplyEngine()
    {
        if (currentEngine) Destroy(currentEngine);

        if (engineMountPoint && engineModels.Length > engineIndex)
        {
            currentEngine = Instantiate(engineModels[engineIndex], engineMountPoint);
            currentEngine.transform.localPosition = Vector3.zero;
            currentEngine.transform.localRotation = Quaternion.identity;
        }
    }

    void UpdateUI()
    {
        if (carNameText) carNameText.text = currentCar.name;
        if (engineNameText) engineNameText.text = "Двигатель: " + engineModels[engineIndex].name;
        if (tireNameText) tireNameText.text = "Шины: " + tireMaterials[tireIndex].name;
    }

    // ---------- Панели ----------
    public void OpenPanel(GameObject panel)
    {
        carPanel.SetActive(false);
        enginePanel.SetActive(false);
        tirePanel.SetActive(false);
        colorPanel.SetActive(false);

        if (panel) panel.SetActive(true);
    }

    // ---------- Завершение выбора ----------
    public void ConfirmSelectionAndGo()
    {
        savedCarIndex = carIndex;
        savedColorIndex = colorIndex;
        savedTireIndex = tireIndex;
        savedEngineIndex = engineIndex;

        SceneManager.LoadScene("track_1");
    }

    // ---------- Финальная сцена ----------
    void LoadFinalCar()
    {
        GameObject car = Instantiate(cars[savedCarIndex], carSpawnPoint.position, Quaternion.identity);
        carRenderer = car.GetComponentInChildren<Renderer>();
        ApplyFinalColor(carRenderer);
        ApplyFinalEngine(car.transform);
        ApplyFinalTires(car.GetComponentsInChildren<MeshRenderer>());
        car.AddComponent<SpinObject>();
    }

    void ApplyFinalColor(Renderer renderer)
    {
        if (renderer && savedColorIndex < carColors.Length)
            renderer.material = carColors[savedColorIndex];
    }

    void ApplyFinalEngine(Transform mount)
    {
        if (engineModels.Length > savedEngineIndex)
        {
            GameObject engine = Instantiate(engineModels[savedEngineIndex], mount);
            engine.transform.localPosition = Vector3.zero;
            engine.transform.localRotation = Quaternion.identity;
        }
    }

    void ApplyFinalTires(MeshRenderer[] renderers)
    {
        foreach (var r in renderers)
        {
            var mats = r.materials;
            if (mats.Length > 1 && savedTireIndex < tireMaterials.Length)
            {
                mats[3] = tireMaterials[savedTireIndex];
                r.materials = mats;
            }
        }
    }
}

// Вращение мышью
public class SpinObject : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, -rotX, Space.World);
        }
    }
}
