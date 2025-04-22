using UnityEngine;
using System.Collections;

public class ObjectPreviewer : MonoBehaviour
{
    public GameObject associatedPanel;
    public GameObject carSelectionPanel;

    public float previewDistance = 2f;
    public float moveSpeed = 5f;
    public float returnSpeed = 3f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isPreviewing = false;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                if (!isMoving)
                {
                    if (isPreviewing)
                        StartCoroutine(MoveBack());
                    else
                        StartCoroutine(MoveToCamera());
                }
            }
        }
    }

    IEnumerator MoveToCamera()
    {
        isMoving = true;
        ObjectPreviewManager.Instance.RegisterPreview(this);

        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * previewDistance;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.3f);
            yield return null;
        }

        transform.position = targetPos;
        isPreviewing = true;
        isMoving = false;

        if (associatedPanel != null)
            associatedPanel.SetActive(true);
    }

    IEnumerator MoveBack()
    {
        isMoving = true;

        if (associatedPanel != null && associatedPanel != carSelectionPanel)
            associatedPanel.SetActive(false);

        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, originalPosition, ref velocity, 0.3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isPreviewing = false;
        isMoving = false;

        ObjectPreviewManager.Instance.ClearPreview(this);
    }

    public void ForceReturn()
    {
        StopAllCoroutines();
        StartCoroutine(MoveBack());
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

        ObjectPreviewManager.Instance.ClearPreview(this);
    }
}
