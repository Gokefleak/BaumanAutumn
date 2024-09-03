using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class test : MonoBehaviour
{
    private Renderer[] renderers;
    private TextMeshPro[] textMeshes;
    public float fadeDuration = 1.0f;
    private Color[] initialColors;
    private Color[] initialTextColors;
    public bool hide = false;
    public bool show = false;

    public void Initialize(float duration, bool display)
    {
        fadeDuration = duration;
        show = display;
    }

    void Update()
    {
        if (show == true)
        {
            controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
            fadeDuration = fadeDuration / forsaves.boostskipause;
            show = false;
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
        if (hide == true)
        {
            controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
            fadeDuration = fadeDuration / forsaves.boostskipause;
            hide = false;
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        textMeshes = GetComponentsInChildren<TextMeshPro>();

        initialColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                initialColors[i] = renderers[i].material.color;
                renderers[i].material.color = new Color(initialColors[i].r, initialColors[i].g, initialColors[i].b, 0);
            }
        }

        // Сохраняем начальные цвета TextMeshPro
        initialTextColors = new Color[textMeshes.Length];
        for (int i = 0; i < textMeshes.Length; i++)
        {
            initialTextColors[i] = textMeshes[i].color;
            // Изначально делаем текст полностью прозрачным
            textMeshes[i].color = new Color(initialTextColors[i].r, initialTextColors[i].g, initialTextColors[i].b, 0);
        }
    }

    private IEnumerator FadeOut()
    {
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    Color color = initialColors[i];
                    renderers[i].material.color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, progress));
                }
            }

            for (int i = 0; i < textMeshes.Length; i++)
            {
                Color color = initialTextColors[i];
                textMeshes[i].color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, progress));
            }

            progress += rate * Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                Color color = initialColors[i];
                renderers[i].material.color = new Color(color.r, color.g, color.b, 0);
            }
        }

        for (int i = 0; i < textMeshes.Length; i++)
        {
            Color color = initialTextColors[i];
            textMeshes[i].color = new Color(color.r, color.g, color.b, 0);
        }
    }

    private IEnumerator FadeIn()
    {
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    Color color = initialColors[i];
                    renderers[i].material.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0, color.a, progress));
                }
            }

            for (int i = 0; i < textMeshes.Length; i++)
            {
                Color color = initialTextColors[i];
                textMeshes[i].color = new Color(color.r, color.g, color.b, Mathf.Lerp(0, color.a, progress));
            }

            progress += rate * Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                Color color = initialColors[i];
                renderers[i].material.color = new Color(color.r, color.g, color.b, color.a);
            }
        }

        for (int i = 0; i < textMeshes.Length; i++)
        {
            Color color = initialTextColors[i];
            textMeshes[i].color = new Color(color.r, color.g, color.b, color.a);
        }
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
}
