using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [Header("Path Following")]
    public Transform Path;
    public float maxSteerAngle = 90f;
    public float maxMotorTorque = 300f;
    public float currentSpeed;
    public float maxSpeed = 3000f;
    public float waypointThreshold = 1f;
    public float steeringSmoothness = 5f;
    public float startDelay = 3f; // Добавлена задержка перед стартом

    [Header("Wheels")]
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public Vector3 centerOfMass = new Vector3(0, -0.5f, 0);

    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition = new Vector3(0, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.3f;
    public float frontSensorAngle = 30f;
    public LayerMask obstacleMask;

    private List<Transform> nodes;
    private int currentNode = 0;
    private bool avoiding = false;
    private float avoidMultiplier = 0f;
    private bool canMove = false; // Флаг разрешения движения
    private float startTimer; // Таймер задержки

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        InitializePath();
        startTimer = startDelay; // Инициализация таймера
    }

    void FixedUpdate()
    {
        // Обработка задержки перед стартом
        if (!canMove)
        {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0)
            {
                canMove = true;
            }
            return;
        }

        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }

    private void InitializePath()
    {
        Transform[] pathTransforms = Path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != Path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    private void ApplySteer()
    {
        if (avoiding)
        {
            WheelFL.steerAngle = maxSteerAngle * avoidMultiplier;
            WheelFR.steerAngle = maxSteerAngle * avoidMultiplier;
        }
        else
        {
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            WheelFL.steerAngle = Mathf.Lerp(WheelFL.steerAngle, newSteer, Time.deltaTime * steeringSmoothness);
            WheelFR.steerAngle = Mathf.Lerp(WheelFR.steerAngle, newSteer, Time.deltaTime * steeringSmoothness);
        }
    }

    private void Drive()
    {
        if (!canMove) 
        {
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
            return;
        }

        currentSpeed = 2 * Mathf.PI * WheelFL.radius * WheelFL.rpm * 60 / 1000;
        
        if (currentSpeed < maxSpeed)
        {
            float torque = maxMotorTorque * (1 - currentSpeed / maxSpeed);
            WheelFL.motorTorque = torque;
            WheelFR.motorTorque = torque;
        }
        else
        {
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < waypointThreshold)
        {
            currentNode = (currentNode == nodes.Count - 1) ? 0 : currentNode + 1;
        }
    }

    private void Sensors()
{
    if (!canMove) return;

    Vector3 sensorStartPos = transform.position;
    sensorStartPos += transform.forward * frontSensorPosition.z;
    sensorStartPos += transform.up * frontSensorPosition.y;

    avoidMultiplier = 0f;
    avoiding = false;

    // Удалены повторные объявления переменных (были строки 134-137)

    // Front center sensor
    if (Physics.Raycast(sensorStartPos, transform.forward, out RaycastHit hit, sensorLength, obstacleMask))
    {
        if (!hit.collider.CompareTag("Terrain"))
        {
            avoiding = true;
            avoidMultiplier = hit.normal.x < 0 ? -1f : 1f;
        }
    }

    // Front right sensor
    Vector3 rightSensorPos = sensorStartPos + transform.right * frontSideSensorPosition;
    if (Physics.Raycast(rightSensorPos, transform.forward, out hit, sensorLength, obstacleMask))
    {
        if (!hit.collider.CompareTag("Terrain"))
        {
            avoiding = true;
            avoidMultiplier -= 1f;
        }
    }
    // Front right angle sensor
    else if (Physics.Raycast(rightSensorPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength, obstacleMask))
    {
        if (!hit.collider.CompareTag("Terrain"))
        {
            avoiding = true;
            avoidMultiplier -= 0.5f;
        }
    }

    // Front left sensor
    Vector3 leftSensorPos = sensorStartPos - transform.right * frontSideSensorPosition;
    if (Physics.Raycast(leftSensorPos, transform.forward, out hit, sensorLength, obstacleMask))
    {
        if (!hit.collider.CompareTag("Terrain"))
        {
            avoiding = true;
            avoidMultiplier += 1f;
        }
    }
    // Front left angle sensor
    else if (Physics.Raycast(leftSensorPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength, obstacleMask))
    {
        if (!hit.collider.CompareTag("Terrain"))
        {
            avoiding = true;
            avoidMultiplier += 0.5f;
        }
    }
}
    private void OnDrawGizmos()
    {
        if (nodes != null && nodes.Count > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(nodes[currentNode].position, 0.5f);

            for (int i = 0; i < nodes.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(nodes[i].position, 0.3f);
                if (i < nodes.Count - 1)
                {
                    Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
                }
                else
                {
                    Gizmos.DrawLine(nodes[i].position, nodes[0].position);
                }
            }
        }
    }
}