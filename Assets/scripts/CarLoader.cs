using UnityEngine;

public class CarLoader : MonoBehaviour
{
    [Header("Car Options")]
    public GameObject[] carModels; // Массив моделей машин (должен соответствовать массиву из CarManager)
    public Material[] carColors; // Массив цветов машин
    public Material[] tireMaterials; // Материалы шин
    public MeshRenderer[] tireRenderers; // Рендереры шин
    public Renderer carRenderer; // Рендерер кузова машины

    private void Start()
    {
        LoadCarSettings();
    }

    private void LoadCarSettings()
    {
        // Загружаем сохраненные индексы
        int savedCarIndex = PlayerPrefs.GetInt("CarIndex", 0);
        int savedColorIndex = PlayerPrefs.GetInt($"ColorIndex_{savedCarIndex}", 0);
        int savedTireIndex = PlayerPrefs.GetInt("TireIndex", 0);

        // Активируем выбранную модель машины
        for (int i = 0; i < carModels.Length; i++)
        {
            carModels[i].SetActive(i == savedCarIndex);
        }

        // Применяем цвет
        if (carRenderer != null && carColors.Length > savedColorIndex)
        {
            Material[] mats = carRenderer.materials;
            
            if (savedCarIndex == 0)
            {
                // Для первой машины меняем два материала (индексы 0 и 3)
                if (mats.Length > 0) mats[0].color = carColors[savedColorIndex].color;
                if (mats.Length > 3) mats[3].color = carColors[savedColorIndex].color;
            }
            else
            {
                // Для остальных машин меняем только первый материал
                if (mats.Length > 0) mats[0].color = carColors[savedColorIndex].color;
            }
            
            carRenderer.materials = mats;
        }
        

        // Применяем шины
        if (tireRenderers != null && tireMaterials.Length > savedTireIndex)
        {
            foreach (var renderer in tireRenderers)
            {
                Material[] mats = renderer.materials;

                if (mats.Length > 1)
                {
                    mats[3] = tireMaterials[savedTireIndex];
                    renderer.materials = mats;
                }
            }
        }
    }
}