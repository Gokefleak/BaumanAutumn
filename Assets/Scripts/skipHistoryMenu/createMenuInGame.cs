using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMenuInGame : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
    public GameObject menu;
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
        if (!spawn && !stopPrint.menuOpen)
        {

            stopPrint.menuOpen = true;
            spawn = true;
            Instantiate(menu, spawnPosition, Quaternion.identity);
            
        }
    }
}