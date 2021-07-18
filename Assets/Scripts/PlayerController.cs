using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class PlayerController : SlimeFighter
{
    
    public int staminaCostDash = 2;

    Camera cam;
    
    public UIStaminaBar staminaBar;
    public UIHealthBar healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        CommonStart();

        cam = Camera.main;
        Application.targetFrameRate = 60;

        staminaBar.maxValue = maxStamina;
        staminaBar.SetValue(maxStamina);

        healthBar.maxValue = maxHp;
        healthBar.SetValue(maxHp);

        initialScale = gameObject.transform.localScale;
    }

    void Update()
    {
        
        CommonUpdate();

        stamina += Time.deltaTime;

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        if (!IsStunned && stamina >= staminaCostDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 move = new Vector2(horizontal, vertical);

            move.Normalize();

            //physics2d.MovePosition(position);
            physics2d.AddForce(dashSpeed * move * physics2d.mass, ForceMode2D.Impulse);
            weapon.physics2d.AddForce(dashSpeed * move * weapon.physics2d.mass, ForceMode2D.Impulse);

            stamina -= staminaCostDash;
          
        }

        staminaBar.SetValue((int)Mathf.Floor(stamina));
        actionStaminaBar.SetValue(stamina);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        Move(move);

    }

    

    protected override bool WeaponIsEnemy(SlimeWeapon slimeWeapon)
    {
        return slimeWeapon is EnemyWeaponController;
    }

    protected override void DisplayHP()
    {
        healthBar.SetValue((int)hp);
        actionHealthBar.SetValue(hp);

        gameObject.transform.localScale = CurrentScale * initialScale;
    }

}
