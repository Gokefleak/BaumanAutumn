using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goSave : MonoBehaviour
{
    public Vector3 targetPosition;  // ������� �������
    public Vector3 targetScale;     // ������� �������
    public float duration = 2.0f;   // �����, �� ������� ����������� ������ ���������
    public float delay = 2.0f;      // �������� ����� �������

    private Renderer objectRenderer; // Renderer ��� ��������� �����

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // ��������� ����������� ��� ��������� �����, ����� ����������� � ��������� �������� � ���������
            StartCoroutine(FadeToYellowAndMove(targetPosition, targetScale, duration, delay));
        }
        else
        {
            Debug.LogWarning("�� ������� ��� ���������� Renderer, ��������� ����� ����������.");
        }
    }

    IEnumerator FadeToYellowAndMove(Vector3 targetPosition, Vector3 targetScale, float duration, float delay)
    {
        // ��������� ���� �������
        Color startColor = objectRenderer.material.color;
        // ������� ���� - ����������
        Color targetColor = new Color(1f, 1f, 0.5f, startColor.a); // ������-������

        float fadeDuration = 2.0f; // ����� ��� ��������� �����
        float elapsedTime = 0;

        // ��������� ����� �� �������
        while (elapsedTime < fadeDuration)
        {
            objectRenderer.material.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �� ���������� �����
        }

        // ��������, ��� ���� ����� ���������� �� �������
        objectRenderer.material.color = targetColor;

        // �������� ����� ������� ����������� � ��������� ��������
        yield return new WaitForSeconds(delay);

        // ����� �������� �������
        elapsedTime = 0;
        Vector3 startPosition = transform.position; // ��������� �������
        Vector3 startScale = transform.localScale;  // ��������� �������

        while (elapsedTime < duration)
        {
            // ������������ �������
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // ������������ ��������
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �� ���������� �����
        }

        // ������������ ������ ��������� � �������� ��������
        transform.position = targetPosition;
        transform.localScale = targetScale;
    }
}
