using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class history : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0f, 0.28f, 0f);
    public GameObject histor;
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            Push();
        }
    }
    public bool isMouseOver = false;
    public bool spawn = false;
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
        controler stopPrint = GameObject.Find("Main Camera").GetComponent<controler>();
        test hideWindow = GameObject.Find("dialogWindows").GetComponent<test>();
        if (!spawn && !stopPrint.menuOpen)
        {
            hideWindow.hide = true;
            hideWindow.fadeDuration = 0.5f;
            stopPrint.menuOpen = true;
            spawn = true;
            Instantiate(histor, spawnPosition, Quaternion.identity);

        }
    }
}
