using UnityEngine;
using System.Collections;

public class ObjectPrev : MonoBehaviour
{
    public GameObject associatedPanel;
    public GameObject carSelectionPanel;

    public float previewDistance = 2f;
    public float moveSpeed = 5f;
    public float returnSpeed = 3f;
    public float rotationSpeed = 0.5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isPreviewing = false;
    private bool isMoving = false;
    private bool isReturningRotation = false;

    private static ObjectPrev activePreview;

    private Vector3 lastMousePosition;
   
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Клик на объект
        if (Input.GetMouseButtonDown(0) && !isPreviewing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                if (!isMoving)
                    StartCoroutine(MoveToCamera());
            }
        }

        // Вращение в превью
        if (isPreviewing && !isMoving)
        {
            if (Input.GetMouseButtonDown(0))
                lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                float rotX = -delta.y * rotationSpeed * Time.deltaTime;
                float rotY = delta.x * rotationSpeed * Time.deltaTime;

                transform.Rotate(Camera.main.transform.up, rotY, Space.World);
                transform.Rotate(Camera.main.transform.right, rotX, Space.World);

                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(RestoreRotation());
            }
        }
    }

    IEnumerator MoveToCamera()
    {
        if (activePreview != null && activePreview != this)
            activePreview.Deactivate();

        activePreview = this;

        isMoving = true;
        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * previewDistance;

        Vector3 startPos = transform.position;
        float journeyLength = Vector3.Distance(startPos, targetPos);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float frac = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPos, targetPos, frac);
            yield return null;
        }

        transform.position = targetPos;
        isPreviewing = true;
        isMoving = false;

        if (carSelectionPanel != null)
            carSelectionPanel.SetActive(false);
        if (associatedPanel != null)
            associatedPanel.SetActive(true);
    }

    IEnumerator RestoreRotation()
    {
        if (isReturningRotation) yield break;
        isReturningRotation = true;

        Quaternion startRot = transform.rotation;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
        isReturningRotation = false;
    }

    public void Deactivate()
    {
        if (!isPreviewing || isMoving) return;
        StartCoroutine(MoveBack());
    }

    IEnumerator MoveBack()
    {
        isMoving = true;

        if (associatedPanel != null)
            associatedPanel.SetActive(false);
        if (carSelectionPanel != null)
            carSelectionPanel.SetActive(true);

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float journeyLength = Vector3.Distance(startPos, originalPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * returnSpeed;
            float frac = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPos, originalPosition, frac);
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, frac);
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        isPreviewing = false;
        isMoving = false;

        if (activePreview == this)
            activePreview = null;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isPreviewing = false;
        isMoving = false;

        if (associatedPanel != null && associatedPanel != carSelectionPanel)
            associatedPanel.SetActive(false);

        if (activePreview == this)
            activePreview = null;
    }

    public void ReturnFromButton()
    {
        if (isPreviewing && !isMoving)
            StartCoroutine(MoveBack());
    }

    public static ObjectPrev GetActivePreview()
    {
        return activePreview;
    }
}
