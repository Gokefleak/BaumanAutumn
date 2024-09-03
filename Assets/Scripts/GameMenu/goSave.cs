using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goSave : MonoBehaviour
{
    public Vector3 targetPosition;  // Целевая позиция
    public Vector3 targetScale;     // Целевой масштаб
    public float duration = 2.0f;   // Время, за которое перемещение должно произойти
    public float delay = 2.0f;      // Задержка перед началом

    private Renderer objectRenderer; // Renderer для изменения цвета

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // Запускаем сопрограмму для изменения цвета, затем перемещения и изменения масштаба с задержкой
            StartCoroutine(FadeToYellowAndMove(targetPosition, targetScale, duration, delay));
        }
        else
        {
            Debug.LogWarning("На объекте нет компонента Renderer, изменение цвета невозможно.");
        }
    }

    IEnumerator FadeToYellowAndMove(Vector3 targetPosition, Vector3 targetScale, float duration, float delay)
    {
        // Начальный цвет объекта
        Color startColor = objectRenderer.material.color;
        // Целевой цвет - желтоватый
        Color targetColor = new Color(1f, 1f, 0.5f, startColor.a); // Светло-желтый

        float fadeDuration = 2.0f; // Время для изменения цвета
        float elapsedTime = 0;

        // Изменение цвета до желтого
        while (elapsedTime < fadeDuration)
        {
            objectRenderer.material.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем до следующего кадра
        }

        // Убедимся, что цвет точно установлен на целевой
        objectRenderer.material.color = targetColor;

        // Ожидание перед началом перемещения и изменения масштаба
        yield return new WaitForSeconds(delay);

        // Сброс счетчика времени
        elapsedTime = 0;
        Vector3 startPosition = transform.position; // Начальная позиция
        Vector3 startScale = transform.localScale;  // Начальный масштаб

        while (elapsedTime < duration)
        {
            // Интерполяция позиции
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // Интерполяция масштаба
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Ждем до следующего кадра
        }

        // Обеспечиваем точное попадание в конечные значения
        transform.position = targetPosition;
        transform.localScale = targetScale;
    }
}
