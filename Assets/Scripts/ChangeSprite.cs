using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ChangeSprite : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;

    private SpriteRenderer spriteRenderer;
    public bool change = false;
    public bool work = true;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (work)
        {
            if (change && Input.GetMouseButtonUp(0))
            {
                spriteRenderer.sprite = defaultSprite;
            }
            if (change && Input.GetMouseButtonDown(0))
            {
                spriteRenderer.sprite = hoverSprite;
            }
        }
        
    }
    public void OnMouseEnter()
    {
        change = true;
        spriteRenderer.sprite = hoverSprite;
    }
    public void OnMouseExit()
    {
        change = false;
        spriteRenderer.sprite = defaultSprite;
    }

}
