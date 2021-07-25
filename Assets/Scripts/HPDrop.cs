using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HPDrop : MonoBehaviour
{

    public int value = 1;

    public SlimeTrail trail;

    Rigidbody2D physics2D;
    new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        physics2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(Mathf.Pow(value,0.5f), Mathf.Pow(value,0.5f));
        
    }

    public void Launch(float velocity, Vector2 direction)
    {
        physics2D.AddForce(velocity * direction, ForceMode2D.Impulse);
    }

    public void SetColor(Color color)
    {
        renderer.color = color;
        Color trailColor = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, color.a);
        trail.SetColor(trailColor);
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        trail.SetWidth(Mathf.Pow(newValue, 0.5f) / 6.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SlimeFighter slimeFighter = other.gameObject.GetComponent<SlimeFighter>();


        if (slimeFighter != null && !slimeFighter.IsInvincible)
        {
            slimeFighter.Heal(value);

            Destroy(gameObject);

        }

    }
}
