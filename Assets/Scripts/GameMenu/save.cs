using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Rendering;
using TMPro;

public class save : MonoBehaviour
{
    public float fadeDistance = 0.5f; // Расстояние, через которое объект начнет исчезать
    public float destroyDistance = 1f; 
    public float moveSpeed = 1f; // Скорость движения объекта

    private Vector3 lastMousePosition;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float distanceTraveled = 0f;
    void Start()
    {
        lastMousePosition = Input.mousePosition;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        StartCoroutine(ExecuteAfterTime(10f));

    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        string name = "TemporarySaving";
        SaveGame(name);
    }
    void Update()
    {
        /*Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));
        Vector3 direction = Vector3.zero;

        // Определяем направление движения объекта
        if (mousePosition.x > lastMousePosition.x)
        {
            direction = Vector3.right;
        }
        else if (mousePosition.x < lastMousePosition.x)
        {
            direction = Vector3.left;
        }

        // Двигаем объект
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Обновляем расстояние, пройденное объектом
        distanceTraveled += (worldMousePosition - transform.position).magnitude;

        // Плавное исчезновение объекта
        if (distanceTraveled >= fadeDistance)
        {
            float fadeAmount = 1 - ((distanceTraveled - fadeDistance) / (destroyDistance - fadeDistance));
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Clamp01(fadeAmount));
        }

        // Уничтожаем объект, если он прошел достаточное расстояние
        if (distanceTraveled >= destroyDistance)
        {
            Destroy(gameObject);
        }

        lastMousePosition = mousePosition;*/
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            Vector3 direction = Vector3.zero;

            // Определяем направление движения объекта
            if (mousePosition.x > lastMousePosition.x)
            {
                direction = Vector3.right;
            }
            else if (mousePosition.x < lastMousePosition.x)
            {
                direction = Vector3.left;
            }

            // Двигаем объект
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Обновляем расстояние, пройденное объектом
            distanceTraveled += (worldMousePosition - transform.position).magnitude;

            // Плавное исчезновение объекта
            if (distanceTraveled >= fadeDistance)
            {
                float fadeAmount = 1 - ((distanceTraveled - fadeDistance) / (destroyDistance - fadeDistance));
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Clamp01(fadeAmount));
            }

            // Уничтожаем объект, если он прошел достаточное расстояние
            if (distanceTraveled >= destroyDistance)
            {
                Push();
            }

            lastMousePosition = mousePosition;
        }
        if (isMouseOver && Input.GetMouseButtonUp(0))
        {
            Push();
        }
    }
    public string saveName;
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
        SaveGame(saveName);
    }
    void SaveGame(string Name)
    {
        controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + '/' + Name + ".dat");
        SaveData data = new SaveData();
        data.spawnDictionary = forsaves.spawnDictionary;
        data.pointDictionary = forsaves.pointDictionary;
        data.history = forsaves.history;
        data.oldName = forsaves.oldName;
        data.question = forsaves.question;
        data.saveQuestion = forsaves.saveQuestion;
        data.rand = forsaves.rand;
        data.oldIndex = forsaves.oldIndex;
        data.currentIndex = forsaves.currentIndex;
        data.show = forsaves.show;
        data.hide = forsaves.hide;
        data.fileName = forsaves.fileName;
        data.clipName = forsaves.clipName;
        data.VolumeNum = forsaves.VolumeNum;
        data.SpeedNum = forsaves.SpeedNum;
        data.RGB = forsaves.RGB;
        bf.Serialize(file, data);
        file.Close();
        Destroy(gameObject);
    }
}
