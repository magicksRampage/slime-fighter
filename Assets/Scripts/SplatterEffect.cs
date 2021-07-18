using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterEffect : MonoBehaviour
{
    new SpriteRenderer renderer;

    public float lifeTime = 2.0f;

    float currentLife = 0.0f;



    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        currentLife = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentLife -= Time.deltaTime;

        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }

        Color currentColor = renderer.color;

        currentColor.a = currentLife / lifeTime;

        SetColor(currentColor);
    }

    public void SetColor(Color color)
    {
        renderer.color = color;
    }
}
