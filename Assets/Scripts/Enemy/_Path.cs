using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Path : MonoBehaviour
{
    public Color pathLineColor = Color.yellow;  // Было: lineColor
    private List<Transform> pathPoints = new List<Transform>();  // Было: nodes

    void OnDrawGizmosSelected()
    {
        Gizmos.color = pathLineColor;

        Transform[] childTransforms = GetComponentsInChildren<Transform>();  // Было: pathTransforms
        pathPoints = new List<Transform>();

        for (int i = 0; i < childTransforms.Length; i++)
        {
            if (childTransforms[i] != transform)
            {
                pathPoints.Add(childTransforms[i]);
            }
        }

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 currentPoint = pathPoints[i].position;  // Было: currentNode
            Vector3 previousPoint = Vector3.zero;           // Было: previousNode

            if (i > 0)
            {
                previousPoint = pathPoints[i - 1].position;
            }
            else if (i == 0 && pathPoints.Count > 1)
            {
                previousPoint = pathPoints[pathPoints.Count - 1].position;
            }

            Gizmos.DrawLine(previousPoint, currentPoint);
            Gizmos.DrawWireSphere(currentPoint, 0.3f);
        }
    }
}
