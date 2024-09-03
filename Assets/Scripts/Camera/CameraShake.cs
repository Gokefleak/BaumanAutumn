using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0f; // Длительность дрожания
    public float shakeMagnitude = 0.1f; // Амплитуда дрожания
    public float dampingSpeed = 1.0f; // Скорость затухания дрожания

    private Vector3 originalPos; // Исходная позиция камеры
    private float currentShakeDuration = 0f;

    void OnEnable()
    {
        originalPos = transform.localPosition; // Сохраняем исходное положение камеры
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            // Случайное смещение позиции камеры
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;

            // Уменьшаем длительность дрожания
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Восстанавливаем позицию камеры
            currentShakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }

    // Метод для запуска дрожания камеры
    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeDuration = duration;
    }
}
