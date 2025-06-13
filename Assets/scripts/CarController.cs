using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        if (!canDrive) return; // ← Блокируем управление до разрешения

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    [SerializeField] private float flipCheckDelay = 3f; // Задержка перед воскрешением
    [SerializeField] private float flipAngleThreshold = 60f; // Допустимый наклон
    private float flipTimer = 0f;
    public bool canDrive = false; // <- управление будет включено только после старта


    private void Update()
    {
        CheckIfFlipped();
    }

    private void CheckIfFlipped()
    {
        float upDot = Vector3.Dot(transform.up, Vector3.up);

        // Если машина перевернулась (up вектор направлен вниз или сильно вбок)
        if (upDot < Mathf.Cos(flipAngleThreshold * Mathf.Deg2Rad))
        {
            flipTimer += Time.deltaTime;

            if (flipTimer >= flipCheckDelay)
            {
                ResetCarRotation();
                flipTimer = 0f;
            }
        }
        else
        {
            flipTimer = 0f; // Сброс таймера, если машина в порядке
        }
    }

    private void ResetCarRotation()
    {
        Vector3 position = transform.position;
        position.y += 1f; // Поднять немного над землёй

        transform.position = position;

        // Повернуть строго вверх, сохранить текущий поворот по Y
        Vector3 upRotation = new Vector3(0f, transform.eulerAngles.y, 0f);
        transform.eulerAngles = upRotation;

        // Также можно сбросить скорость, если нужно
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    private void Start()
    {
        // Загружаем настройки двигателя
        LoadEngineSettings();
    }

    private void LoadEngineSettings()
    {
        int savedEngineIndex = PlayerPrefs.GetInt("EngineIndex", 0);
        
        // Получаем спецификации двигателя из CarManager
        CarManager.EngineSpecs engine = CarManager.Instance.engineSpecs[savedEngineIndex];
        
        // Конвертируем л.с. в значение, подходящее для motorForce (может потребоваться настройка)
        motorForce = engine.torque * 2f; // Множитель можно настроить под вашу физику
        
        // Также можно использовать power, если нужно
        // breakForce можно тоже сделать зависимым от двигателя
        breakForce = engine.torque * 3f;
    }

}