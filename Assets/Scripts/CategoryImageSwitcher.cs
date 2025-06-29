using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class CategoryGroup
{
    public string categoryName;
    public GameObject[] images = new GameObject[2]; // 2 изображения на каждую категорию
}

public class CategoryImageSwitcher : MonoBehaviour
{
    [Header("Категории")]
    public List<CategoryGroup> categories = new List<CategoryGroup>(); // 5 категорий по 2 картинки

    [Header("Кнопки категорий")]
    public Button[] categoryButtons; // 5 кнопок: основы, настройка и т.д.

    [Header("Кнопки переключения")]
    public Button leftButton;
    public Button rightButton;

    
    private int currentCategoryIndex = 0;
    private int currentImageIndex = 0;
        void Start()
    {
        // Назначаем слушатели на кнопки категорий
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int index = i; // сохранить индекс
            categoryButtons[i].onClick.AddListener(() => OnCategorySelected(index));
        }

        // Назначаем кнопки листания
        leftButton.onClick.AddListener(ShowPreviousImage);
        rightButton.onClick.AddListener(ShowNextImage);

        // Скрываем все картинки
        HideAllImages();

        // По умолчанию выбрать первую категорию
        OnCategorySelected(0);
    }

    void OnCategorySelected(int categoryIndex)
    {
        HideAllImages();

        currentCategoryIndex = categoryIndex;
        currentImageIndex = 0;

        var images = categories[categoryIndex].images;
        if (images.Length > 0 && images[0] != null)
        {
            images[0].SetActive(true);
        }
        

    }

    void ShowNextImage()
    {
        var images = categories[currentCategoryIndex].images;
        if (images.Length <= 1) return;

        images[currentImageIndex].SetActive(false);
        currentImageIndex = (currentImageIndex + 1) % images.Length;
        images[currentImageIndex].SetActive(true);
    }

    void ShowPreviousImage()
    {
        var images = categories[currentCategoryIndex].images;
        if (images.Length <= 1) return;

        images[currentImageIndex].SetActive(false);
        currentImageIndex = (currentImageIndex - 1 + images.Length) % images.Length;
        images[currentImageIndex].SetActive(true);
    }

    void HideAllImages()
    {
        foreach (var category in categories)
        {
            foreach (var img in category.images)
            {
                if (img != null)
                    img.SetActive(false);
            }
        }
    }
}
