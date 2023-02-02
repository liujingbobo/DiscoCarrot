using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Vector2 Dir;
    public float speed;
    public RectTransform rect;
    
    private void FixedUpdate()
    {
        var temp = Dir * speed * Time.deltaTime;
        rect.anchoredPosition = rect.anchoredPosition + new Vector2( temp.x, temp.y);
    }
}
