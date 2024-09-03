using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class historyMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width && Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            CheckObjectUnderCursor();
        }
    }
    void CheckObjectUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.CompareTag("menu"))
            {
                Dest();
            }
        }
        else
        {
            Dest();
        }
    }
    public void Dest()
    {
        controler stopPrint = GameObject.Find("Main Camera").GetComponent<controler>();
        history closeMenu = GameObject.Find("historyBarButton").GetComponent<history>();
        test hideWindow = GameObject.Find("dialogWindows").GetComponent<test>();
        GameObject[] RemoveMenu = GameObject.FindGameObjectsWithTag("menu");
        foreach (GameObject obj in RemoveMenu)
        {
            stopPrint.menuOpen = false;
            closeMenu.spawn = false;
            hideWindow.fadeDuration = 0.5f;
            hideWindow.show = true;
            Destroy(obj);
        }
    }
}


