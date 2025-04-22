using UnityEngine;

public class ObjectPreviewManager : MonoBehaviour
{
    public static ObjectPreviewManager Instance;

    private ObjectPreviewer currentPreviewing;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPreview(ObjectPreviewer previewer)
    {
        if (currentPreviewing != null && currentPreviewing != previewer)
        {
            currentPreviewing.ForceReturn(); // Возвращаем предыдущий объект
        }

        currentPreviewing = previewer;
    }

    public void ClearPreview(ObjectPreviewer previewer)
    {
        if (currentPreviewing == previewer)
            currentPreviewing = null;
    }
}
