using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class SlimeTrail : MonoBehaviour
{
    TrailRenderer renderer;
    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        Color endColor = color;
        //endColor.a = 0;
        renderer.startColor = color;
        renderer.endColor = endColor;
    }

    public void SetWidth(float width)
    {
        renderer.startWidth = width;
        renderer.endWidth = 0;
    }
}
