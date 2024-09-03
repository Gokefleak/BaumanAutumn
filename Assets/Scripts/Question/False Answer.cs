using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FalseAnswer : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }
    void Update()
    {
        controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
        if (isMouseOver && Input.GetMouseButtonDown(0) && forsaves.question && !forsaves.menuOpen)
        {
            Push();
        }
    }
    public int count = 0;
    public string name = "";
    public bool isMouseOver = false;
    public int index = 0;
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
        controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
        if (forsaves.pointDictionary.ContainsKey(name))
        {
            forsaves.pointDictionary[name] -= count;
        }
        else
        {
            forsaves.pointDictionary.Add(name, -count);
        }
        forsaves.oldName = null;
        forsaves.history.Add("Сделан выбор: " + textMeshPro.text);
        forsaves.currentIndex = index;
        forsaves.nextphrase = true;

        GameObject[] objectsToRemove = GameObject.FindGameObjectsWithTag("question");
        foreach (GameObject obj in objectsToRemove)
        {
            test StartDestroy = obj.GetComponent<test>();
            StartDestroy.hide = true;
            Destroy(obj, 1/forsaves.boostskipause);
        }
    }
}
