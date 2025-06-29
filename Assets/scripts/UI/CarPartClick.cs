using UnityEngine;

public class CarPartClick : MonoBehaviour
{
    public Vector3 targetPositionOffset = new Vector3(-24.4f, 0.54f, -12.96f); 
    public float moveSpeed = 5f; 
    private Transform selectedPart;
    private Vector3 originalPosition;
    private bool isMoving = false;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Tire") || hit.transform.CompareTag("Motor"))
                {
                    if (selectedPart != null) 
                    {
                        StartCoroutine(MoveObject(selectedPart, originalPosition));
                    }

                    selectedPart = hit.transform;
                    originalPosition = selectedPart.position;
                    Vector3 targetPosition = originalPosition + targetPositionOffset;
                    StartCoroutine(MoveObject(selectedPart, targetPosition));
                }
            }
        }
    }

    private System.Collections.IEnumerator MoveObject(Transform obj, Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = obj.position;
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            obj.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        obj.position = targetPos;
        isMoving = false;
    }
}