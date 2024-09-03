using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewGame : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0f, 0f, -1.2f);
    public GameObject menu;
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonUp(0))
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
        if (!spawn)
        {
            spawn = true;
            Instantiate(menu, spawnPosition, Quaternion.identity);
        }
    }
}
