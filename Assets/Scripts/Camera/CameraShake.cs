using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0f; // ������������ ��������
    public float shakeMagnitude = 0.1f; // ��������� ��������
    public float dampingSpeed = 1.0f; // �������� ��������� ��������

    private Vector3 originalPos; // �������� ������� ������
    private float currentShakeDuration = 0f;

    void OnEnable()
    {
        originalPos = transform.localPosition; // ��������� �������� ��������� ������
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            // ��������� �������� ������� ������
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;

            // ��������� ������������ ��������
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // ��������������� ������� ������
            currentShakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }

    // ����� ��� ������� �������� ������
    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeDuration = duration;
    }
}
