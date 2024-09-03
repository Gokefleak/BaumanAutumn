using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objects : MonoBehaviour
{
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
    public bool hide = false;
    public bool show = false;
    private Renderer[] renderers;
    private Color[] initialColors;
    public float fadeDuration = 1.0f;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        initialColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                initialColors[i] = renderers[i].material.color;
                renderers[i].material.color = new Color(initialColors[i].r, initialColors[i].g, initialColors[i].b, 0);
            }
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
