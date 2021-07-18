using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ActionBar : MonoBehaviour
{

    SpriteRenderer renderer;
    SpriteMask mask;

    Vector2 initialScale;
    public int maxValue;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        mask = GetComponentInChildren<SpriteMask>();

        initialScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        renderer.color = color;
    }

    public void SetMaxValue(int value)
    {
        maxValue = value;
    }

    public void SetValue(float value)
    {
        gameObject.transform.localScale = new Vector2(value / maxValue * initialScale.x, initialScale.y);
        //mask.transform.localScale = new Vector2(value / maxValue, 1);
        renderer.sortingOrder = (int)value;
    }
}
