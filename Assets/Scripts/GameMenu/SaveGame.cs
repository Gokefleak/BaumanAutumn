using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Rendering;
using TMPro;

public class saveGame : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 cameraPosition = new Vector3(0, 0, -20);
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    public string fileName = "sceneSnapshot.png";
    private Vector3 texturePosition = new Vector3(0, 0, -1);
    void Start()
    {
        GameObject cameraObject = GameObject.Find("Main Camera");
        if (cameraObject != null)
        {
            mainCamera = cameraObject.GetComponent<Camera>();
        }

    }
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonUp(0))
        {
            Push();
        }
    }
    public bool isMouseOver = false;
    public void OnMouseEnter()
    {
        isMouseOver = true;
    }
    public void OnMouseExit()
    {
        isMouseOver = false;
    }
    void Push()
    {
        TakeSnapshot();
        GameMenu stopPrint = GameObject.Find("menuActiveBar(Clone)").GetComponent<GameMenu>();
        stopPrint.Dest(); 
        
    }
    void TakeSnapshot()
    {
        // Сохраняем текущие слои камеры
        LayerMask originalLayerMask = mainCamera.cullingMask;

        // Перемещаем камеру
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.LookAt(Vector3.zero); // Смените на нужную точку взгляда

        // Создаем RenderTexture
        RenderTexture renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        mainCamera.targetTexture = renderTexture;
        Texture2D texture = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);

        // Отключаем рендеринг объектов с тегами "button" и "menu"
        SetRenderingState("button", true);
        SetRenderingState("menu", true);

        // Рендерим сцену и читаем пиксели
        mainCamera.Render();
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        texture.Apply();

        // Сохраняем текстуру как PNG
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, bytes);

        // Очищаем
        RenderTexture.active = null;
        mainCamera.targetTexture = null;
        Destroy(renderTexture);

        // Восстанавливаем слои камеры
        mainCamera.cullingMask = originalLayerMask;
        SetRenderingState("button", false);
        SetRenderingState("menu", false);

        // Создаем объект на сцене с текстурой через SpriteRenderer
        CreateSpriteOnScene(texture, texturePosition);
    }

    void SetRenderingState(string excludeTag, bool enable)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(excludeTag))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = !enable;
                }
            }
        }
    }

    void CreateSpriteOnScene(Texture2D texture, Vector3 position)
    {
        // Создаем спрайт из текстуры
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Создаем новый объект на сцене
        GameObject newObject = new GameObject("SpriteObject");

        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        BoxCollider boxCollider = newObject.AddComponent<BoxCollider>();
        newObject.tag = "save";
        spriteRenderer.sprite = sprite;
        boxCollider.size = spriteRenderer.bounds.size;
        boxCollider.center = spriteRenderer.bounds.center - newObject.transform.position;

        // Устанавливаем позицию объекта на сцене
        newObject.transform.position = position;

        float aspectRatio = (float)texture.width / texture.height;
        newObject.transform.localScale = new Vector3(0.925f, 0.925f, 1f);
        goSave moveAndScaleScript = newObject.AddComponent<goSave>();
        moveAndScaleScript.targetPosition = new Vector3(4.3f, 3.5f, -1f);
        moveAndScaleScript.targetScale = new Vector3(0.2f, 0.2f, 1f);
        save GameSave = newObject.AddComponent<save>();
        GameSave.saveName = "Save" + "1";

        
    }


}

