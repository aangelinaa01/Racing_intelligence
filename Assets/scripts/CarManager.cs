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
    public GameObject repairPanel;

    [Header("Car Movement")]
    public Transform previewPoint;
    private Vector3 carOriginalPosition;
    private Quaternion carOriginalRotation;

    void Start()
    {
        LoadSelection();
        ShowCar(currentCarIndex);
        UpdateUI();
        OpenRepairPanel(); // Панель repairPanel открывается сразу при запуске сцены
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
        ApplyColor();
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
        UpdateUI();
    }

    public void PrevEngine()
    {
        currentEngineIndex = (currentEngineIndex - 1 + engineNames.Length) % engineNames.Length;
        UpdateUI();
    }

    public void NextTire()
    {
        currentTireIndex = (currentTireIndex + 1) % tireNames.Length;
        UpdateUI();
    }

    public void PrevTire()
    {
        currentTireIndex = (currentTireIndex - 1 + tireNames.Length) % tireNames.Length;
        UpdateUI();
    }

    public void SetColor(int index)
    {
        currentColorIndex = index;
        ApplyColor();
        UpdateUI();
    }

    private void ApplyColor()
    {
        if (carRenderer != null && carColors.Length > currentColorIndex)
            carRenderer.material = carColors[currentColorIndex];
    }

    private void UpdateUI()
    {
        carNameText.text = "Модель: " + cars[currentCarIndex].name;
        engineNameText.text = "Двигатель: " + engineNames[currentEngineIndex];
        tireNameText.text = "Шины: " + tireNames[currentTireIndex];
    }

    public void OpenTirePanel()
    {
        SaveCarPosition();
        MoveCarToPreview();
        SetPanelActive(tirePanel);
    }

    public void OpenEnginePanel()
    {
        SaveCarPosition();
        MoveCarToPreview();
        SetPanelActive(enginePanel);
    }

    public void OpenColorPanel()
    {
        SaveCarPosition();
        MoveCarToPreview();
        SetPanelActive(colorPanel);
    }

    public void OpenRepairPanel()
    {
        SaveCarPosition();
        MoveCarToPreview();
        SetPanelActive(repairPanel); // Открывается сразу при запуске сцены
    }

    public void OpenCarSelectionPanel()
    {
        ReturnCarToOriginal();
        SetPanelActive(carSelectionPanel);
    }

    public void ConfirmSelection()
    {
        SaveSelection();
        // Не переходим на главный экран, остаёмся на текущей панели
    }

    void SetPanelActive(GameObject targetPanel)
    {
        carSelectionPanel.SetActive(false);
        tirePanel.SetActive(false);
        enginePanel.SetActive(false);
        colorPanel.SetActive(false);
        repairPanel.SetActive(false);

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

    void SaveCarPosition()
    {
        GameObject currentCar = cars[currentCarIndex];
        carOriginalPosition = currentCar.transform.position;
        carOriginalRotation = currentCar.transform.rotation;
    }

    void MoveCarToPreview()
    {
        GameObject currentCar = cars[currentCarIndex];
        currentCar.transform.position = previewPoint.position;
        currentCar.transform.rotation = previewPoint.rotation;
    }

    void ReturnCarToOriginal()
    {
        GameObject currentCar = cars[currentCarIndex];
        currentCar.transform.position = carOriginalPosition;
        currentCar.transform.rotation = carOriginalRotation;
    }
}
