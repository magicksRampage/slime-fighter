using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int baseDamage = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SlimeFighter fighter = collision.gameObject.GetComponent<SlimeFighter>();

        if (fighter && !fighter.IsInvincible)
        {
            fighter.Damage(baseDamage);
        }
    }
}
