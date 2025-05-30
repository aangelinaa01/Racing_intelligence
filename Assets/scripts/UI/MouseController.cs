using UnityEngine;
using TMPro;

public class MouseController : MonoBehaviour
{
    public Camera mainCamera;
    public TextMeshProUGUI detailNameText;
    public float maxDistance = 100f;
    public LayerMask detailLayer;

    private string lastTag = "";
    private GameObject[] highlightedObjects;

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, detailLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            string nameToShow = GetDetailName(hitObject.name);
            ShowDetailName(nameToShow);

            string currentTag = hitObject.tag;

            if (currentTag != lastTag)
            {
                RemoveHighlightFromAll(); // убрать старую подсветку
                HighlightGroup(currentTag); // подсветить новую группу
                lastTag = currentTag;
            }
        }
        else
        {
            HideDetailName();
            RemoveHighlightFromAll();
            lastTag = "";
        }
        if (ObjectPrev.GetActivePreview() != null)
        {
            HideDetailName();
            RemoveHighlightFromAll();
            return; // отключить наведение при активном подлёте
        }

    }

    void ShowDetailName(string name)
    {
        detailNameText.text = name;
        detailNameText.gameObject.SetActive(true);
        detailNameText.transform.position = Input.mousePosition + new Vector3(10, -10, 0);
    }

    void HideDetailName()
    {
        detailNameText.gameObject.SetActive(false);
    }

    string GetDetailName(string objectName)
    {
        switch (objectName)
        {
            case "FrontLeft":
            case "FrontRight":
            case "RearLeft":
            case "RearRight":
                return "Шины";
            case "Electric motor":
            case "V6":
            case "V8":
                return "Двигатель";
            default:
                return objectName;
        }
    }

    void HighlightGroup(string tag)
    {
        highlightedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in highlightedObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
        }
    }

    void RemoveHighlightFromAll()
    {
        if (highlightedObjects == null) return;

        foreach (GameObject obj in highlightedObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }

        highlightedObjects = null;
    }
}
