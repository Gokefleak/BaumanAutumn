using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skip : MonoBehaviour
{
    public bool isMouseOver = false;
    public void OnMouseEnter()
    {
        isMouseOver = true;
    }
    public void OnMouseExit()
    {
        isMouseOver = false;
    }
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            Push();
        }
    }
    void Push()
    {
        controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
        forsaves.boostskip = !forsaves.boostskip;
        if (forsaves.boostskipause == 1)
        {
            forsaves.boostskipause = 10f;
        }
        else
        {
            forsaves.boostskipause = 1f;
        }
        
    }
}
