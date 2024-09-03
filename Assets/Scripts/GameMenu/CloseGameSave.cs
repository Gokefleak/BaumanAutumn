using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class CloseGameSave : MonoBehaviour
{
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
        controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
        int SaveCurrentIndex = forsaves.currentIndex;
        bool SaveShow = forsaves.show;
        bool SaveHide = forsaves.hide;
        string SaveFileName = forsaves.fileName;
        string SaveClipName = forsaves.clipName;
        float SaveVolumeNum = forsaves.VolumeNum;
        float SaveSpeedNum = forsaves.SpeedNum;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/TemporarySaving.dat");
        SaveData data = new SaveData();
        data.spawnDictionary = forsaves.spawnDictionary;
        data.pointDictionary = forsaves.pointDictionary;
        data.history = forsaves.history;
        data.oldName = forsaves.oldName;
        data.question = forsaves.question;
        data.saveQuestion = forsaves.saveQuestion;
        data.rand = forsaves.rand;
        data.oldIndex = forsaves.oldIndex;
        data.currentIndex = SaveCurrentIndex;
        data.show = SaveShow;
        data.hide = SaveHide;
        data.fileName = SaveFileName;
        data.clipName = SaveClipName;
        data.VolumeNum = SaveVolumeNum;
        data.SpeedNum = SaveSpeedNum;
        data.RGB = forsaves.RGB;
        bf.Serialize(file, data);
        file.Close();
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
