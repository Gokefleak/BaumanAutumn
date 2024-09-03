using UnityEngine;
using TMPro;
using System.Collections;

public class VisibleText : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public float upperLimitY = 3.2f;
    public float lowerLimitY = -2.6f;
    public float transparentAlpha = 0.0f;
    public float opaqueAlpha = 1.0f;
    public float fadeDuration = 1.0f; // ������������ �������� ���������/������������ ������

    private bool isFadingIn = true; // ����, ������������, ���� �� ������� ���������
    private bool isFadingOut = false; // ����, ������������, ���� �� ������� ������������
    private bool isFullyVisible = false; // ����, ������������, ��� ����� ��������� �����

    void Awake()
    {
        textMeshPro.ForceMeshUpdate(); // ��������� ��������� ����� �����, ����� �������� ���������� ���������� ��������
        StartCoroutine(FadeInText());
    }

    void LateUpdate()
    {
        // ��������� ������������ ���������� ������������ ������ ����� ���������� �������� ��������� ��� ������������
        if (isFullyVisible && !isFadingIn && !isFadingOut)
        {
            UpdateTextTransparency(opaqueAlpha);
        }
    }

    IEnumerator FadeInText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAlpha = Mathf.Lerp(transparentAlpha, opaqueAlpha, elapsedTime / fadeDuration);

            UpdateTextTransparency(fadeAlpha);

            yield return null; // ���� �� ���������� �����
        }

        // � ����� ������������� �����
        isFadingIn = false;
        isFullyVisible = true; // ����� ��������� �����
    }

    IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAlpha = Mathf.Lerp(opaqueAlpha, transparentAlpha, elapsedTime / fadeDuration);

            UpdateTextTransparency(fadeAlpha);

            yield return null; // ���� �� ���������� �����
        }

        // � ����� ������������� �����
        isFadingOut = false;
        isFullyVisible = false; // ����� ������ �� �����
    }

    public void StartFadeOut()
    {
        if (!isFadingOut && isFullyVisible)
        {
            isFadingOut = true;
            StartCoroutine(FadeOutText());
        }
    }

    void UpdateTextTransparency(float fadeAlpha)
    {
        for (int i = 0; i < textMeshPro.textInfo.characterCount; i++)
        {
            var charInfo = textMeshPro.textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            float charMidY = (charInfo.bottomLeft.y + charInfo.topRight.y) / 2;
            Vector3 charWorldPos = textMeshPro.transform.TransformPoint(new Vector3(0, charMidY, 0));

            float alpha = fadeAlpha;

            if (charWorldPos.y > upperLimitY)
            {
                float t = Mathf.InverseLerp(upperLimitY, upperLimitY + 1f, charWorldPos.y);
                alpha = Mathf.Lerp(fadeAlpha, transparentAlpha, t);
            }

            if (charWorldPos.y < lowerLimitY)
            {
                float t = Mathf.InverseLerp(lowerLimitY, lowerLimitY - 1f, charWorldPos.y);
                alpha = Mathf.Lerp(fadeAlpha, transparentAlpha, t);
            }

            Color32[] vertexColors = textMeshPro.textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
            int vertexIndex = charInfo.vertexIndex;

            vertexColors[vertexIndex + 0].a = (byte)(alpha * 255);
            vertexColors[vertexIndex + 1].a = (byte)(alpha * 255);
            vertexColors[vertexIndex + 2].a = (byte)(alpha * 255);
            vertexColors[vertexIndex + 3].a = (byte)(alpha * 255);
        }

        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
