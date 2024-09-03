using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
public class load : MonoBehaviour
{
    public Dictionary<string, AudioClip> audioClipDictionary;
    public List<AudioClip> audioClips;
    private AudioSource audioSource;
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
    private bool spawn = false;
    void Push()
    {

        if (File.Exists(Application.persistentDataPath
          + "/TemporarySaving.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/TemporarySaving.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            RemoveAllMusic();
            GameMenu stopPrint = GameObject.Find("menuActiveBar(Clone)").GetComponent<GameMenu>();
            stopPrint.Dest();
            controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
            GameObject[] objectsToRemove = GameObject.FindGameObjectsWithTag("object");
            GameObject[] objectsWithTagQuestion = GameObject.FindGameObjectsWithTag("question");

            foreach (GameObject obj in objectsToRemove)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in objectsWithTagQuestion)
            {
                Destroy(obj);
            }
            GameObject textObject = GameObject.Find("Name");
            TextMeshPro nametext = textObject.GetComponent<TextMeshPro>();
            //nametext.color = new Color(data.RGB[0], data.RGB[1], data.RGB[2]);
            forsaves.oldtext = "";
            forsaves.nowIndex = 0;
            forsaves.isDisplaying = false;
            forsaves.history = data.history;
            forsaves.history.RemoveAt(forsaves.history.Count - 1);
            forsaves.oldName = data.oldName;
            if (forsaves.history[forsaves.history.Count - 1] == data.oldName && data.oldName != "")
            {
                forsaves.history.RemoveAt(forsaves.history.Count - 1);
            }
            TextAsset textAsset = Resources.Load<TextAsset>(data.fileName);
            string fileContent = textAsset.text;
            string[] lines = fileContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            forsaves.lines = lines;

            if (data.show == true)
            {
                test hideWindow = GameObject.Find("dialogWindows").GetComponent<test>();
                hideWindow.show = data.show;
                hideWindow.fadeDuration = 0;
            }
            else
            {
                test hideWindow = GameObject.Find("dialogWindows").GetComponent<test>();
                hideWindow.hide = data.hide;
                hideWindow.fadeDuration = 0;
            }

            if (!data.question)
            {
                forsaves.currentIndex = data.currentIndex - 1;
            }
            else
            {
                forsaves.currentIndex = data.currentIndex;
            }
            
            forsaves.oldIndex = data.oldIndex + 1;
            forsaves.PlayMusic(data.clipName, data.VolumeNum, data.SpeedNum);
            forsaves.pointDictionary = data.pointDictionary;
            if (data.question)
            {
                forsaves.CreateQuestion(data.saveQuestion);
            }
            foreach (var kvp in data.spawnDictionary)
            {
                string spawnName = kvp.Key;
                (float x, float y, float z, float scale, float rotationX, float rotationY, float rotationZ) = kvp.Value;

                Vector3 spawnPosition = new Vector3(x, y, z);
                forsaves.CreateObject(spawnName, spawnPosition, scale, 0f, rotationX, rotationY, rotationZ);
            }
            forsaves.spawnDictionary = data.spawnDictionary;
            if (!data.question)
            {
                forsaves.nextphrase = true;
                if (forsaves.nowprint) { forsaves.loadsaves = true; }
            }
            else 
            {
                forsaves.nextphrase = false;
                forsaves.loadsaves = false;
            }
            
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");

    }
    void RemoveAllMusic()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioS in allAudioSources)
        {
            if (audioS.CompareTag("MainCamera"))
            {
                audioS.clip = null;
            }
        }
    }
}





[Serializable]
class SaveData
{
    public Dictionary<string, (float x, float y, float z, float scale, float rotationX, float rotationY, float rotationZ)> spawnDictionary;
    public Dictionary<string, int> pointDictionary;
    public List<string> history;
    public string oldName;
    public string saveQuestion;
    public bool rand;
    public bool question;
    public int oldIndex;
    public int currentIndex;
    public bool show;
    public bool hide;
    public string fileName;
    public string clipName;
    public float VolumeNum;
    public float SpeedNum;
    public float[] RGB;
}
