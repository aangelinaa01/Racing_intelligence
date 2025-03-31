using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private float tiltAngle = 4f; // Угол наклона в градусах
    [SerializeField] private float tiltSpeed = 2f; // Скорость наклона
    [SerializeField] private float tiltSmoothness = 5f; // Плавность наклона
    
    private Quaternion startRotation;
    private bool tiltingRight = true;
    private float currentTilt = 0f;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Определяем направление наклона
        float targetTilt = tiltingRight ? -tiltAngle : tiltAngle;
        
        // Плавно изменяем текущий наклон
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);
        
        // Создаем поворот на основе текущего наклона
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, currentTilt, 0);
        
        // Плавно применяем поворот
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSmoothness * Time.deltaTime);
        
        // Проверяем, достигнут ли максимальный наклон
        if (Mathf.Abs(currentTilt - targetTilt) < 0.1f)
        {
            tiltingRight = !tiltingRight; // Меняем направление
        }
    }
}