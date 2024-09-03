using UnityEngine;
using TMPro;

public class TextScroll : MonoBehaviour
{
    public float scrollSpeed = 500f;
    public TextMeshPro textMeshPro;
    public float lowerLimit = -2.9f;
    public float upperLimit = 3.5f;
    public float textHeight;
    public float upperEdge;
    public float lowerEdge;

    private Vector2 touchStartPos; 
    private Vector2 touchEndPos;

    private Vector3 velocity = Vector3.zero;
    void Awake()
    {
        controler forsaves = GameObject.Find("Main Camera").GetComponent<controler>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        foreach (string entry in forsaves.history)
        {
            textMeshPro.text += entry + "\n";
        }
        Vector3 newPosition = textMeshPro.transform.position;
        textHeight = textMeshPro.preferredHeight;
        if (Mathf.Abs(upperLimit) + Mathf.Abs(lowerLimit) > textHeight)
        {
            newPosition.y = upperLimit - textHeight / 2;
        }
        else
        {
            newPosition.y = textHeight / 2 + lowerLimit;
        }
        textMeshPro.transform.position = newPosition;
    }
    private void Update()
    {
        if (Mathf.Abs(upperLimit) + Mathf.Abs(lowerLimit) < textHeight)
        {
            ScrollText();
        }
        
    
    }
    public void ScrollText()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
                float touchDelta = touchStartPos.y - touchEndPos.y;
                scroll = touchDelta * 0.01f / 50;
            }
        }

        if (scroll != 0.0f)
        {
            Vector3 newPosition = textMeshPro.transform.position + Vector3.up * scroll * scrollSpeed * Time.deltaTime;

            textHeight = textMeshPro.preferredHeight;
            upperEdge = newPosition.y + textHeight / 2;
            lowerEdge = newPosition.y - textHeight / 2;

            if (upperEdge > upperLimit && lowerEdge > lowerLimit && scroll > 0)
            {
                newPosition.y = upperLimit - textHeight / 2;
            }
            else if (lowerEdge < lowerLimit && upperEdge < upperLimit && scroll < 0)
            {
                newPosition.y = textHeight / 2 + lowerLimit;
            }
            else
            {
                textMeshPro.transform.position = newPosition;
            }
        }
    }
}