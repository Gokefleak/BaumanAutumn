using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCreateNewGame : MonoBehaviour
{
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonUp(0))
        {
            Dest();
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
    public void Dest()
    {
        CreateNewGame spawnNew = GameObject.Find("newGame").GetComponent<CreateNewGame>();
        spawnNew.spawn = false;
        GameObject[] RemoveMenu = GameObject.FindGameObjectsWithTag("menu1");
        foreach (GameObject obj in RemoveMenu)
        {
            Destroy(obj);
        }
    }
}
