using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour
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
    public void Push()
    {
        controler createGame = GameObject.Find("Main Camera").GetComponent<controler>();
        createGame.mainMenu = false;
        CreateNewGame spawnNew = GameObject.Find("newGame").GetComponent<CreateNewGame>();
        spawnNew.spawn = false;
        GameObject[] RemoveMenu1 = GameObject.FindGameObjectsWithTag("menu1");
        foreach (GameObject obj in RemoveMenu1)
        {
            Destroy(obj);
        }
        GameObject[] RemoveMenu = GameObject.FindGameObjectsWithTag("menu");
        foreach (GameObject obj in RemoveMenu)
        {
            Destroy(obj);
        }
    }
}
