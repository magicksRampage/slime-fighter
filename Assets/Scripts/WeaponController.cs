using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponController : SlimeWeapon
{

    Camera cam;
    SpriteRenderer renderer;
    
    public int staminaCostSwing = 1;
    public int staminaCostThrust = 1;
    public float maxThrust = 1.0f;


    float chargeTime = 0;

    float flashTime = 0;
    bool flashOn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
        CommonStart();
        
        cam = Camera.main;
        renderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

        CommonUpdate();

        PlayerController player = GetComponentInParent<PlayerController>();

        if (player.IsStunned)
        {
            ResetCharge();
        }
        else if (player.stamina <= staminaCostSwing * chargeTime)
        {
            player.stamina = 0;
            player.staminaBar.SetValue((int)Mathf.Floor(player.stamina));
            ResetCharge();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse0))
        {
            if (player.stamina > staminaCostThrust * chargeTime)
            {
                timeThrust = maxThrust * chargeTime * staminaCostThrust / player.maxStamina;

                player.stamina -= staminaCostThrust * chargeTime;
                player.staminaBar.SetValue((int)Mathf.Floor(player.stamina));

            }
            ResetCharge();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            if (player.stamina > staminaCostSwing * chargeTime)
            {
                physics2d.AddForce(activeSwing * GetBaseSwing() * chargeTime * staminaCostSwing / player.maxStamina, ForceMode2D.Impulse);

                player.stamina -= staminaCostSwing * chargeTime;
                player.staminaBar.SetValue((int)Mathf.Floor(player.stamina));
            }
            ResetCharge();
        }

        goalDistance = initialDistance + timeThrust;

        float chargeFactor = chargeTime * staminaCostSwing / player.stamina;
        if (chargeFactor > 0.6)
        {
            flashTime -= Time.deltaTime;
            if (flashTime <= 0)
            {
                flashOn = !flashOn;
                flashTime = 0.2f;

                if (chargeFactor > 0.8)
                {
                    flashTime = 0.06f;
                }
            }
        }

        float flashFactor = 1;
        if (flashOn)
        {
            flashFactor = 0.5f;
        }

        renderer.color = new Color(1 - chargeFactor, 1, 1 - chargeFactor, flashFactor);
    }

    void ResetCharge()
    {
        chargeTime = 0;
        flashTime = 0;
        flashOn = false;
    }


    void FixedUpdate()
    {
        PlayerController player = GetComponentInParent<PlayerController>();
        if (!player.IsStunned)
        {
            physics2d.AddForce(GetBaseSwing());

            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                chargeTime += Time.fixedDeltaTime;
            }
        }

        Orbit();


    }

    private Vector3 GetBaseSwing()
    {
        PlayerController player = GetComponentInParent<PlayerController>();

        Vector3 mousePosition = Input.mousePosition;
        Vector3 position = transform.position;


        mousePosition = cam.ScreenToWorldPoint(mousePosition);

        mousePosition.z = transform.position.z;


        Vector3 comPosition = transform.parent.position;
        Vector3 mouseDirection = mousePosition - position;
        Vector3 outDirection = position - comPosition;

        mouseDirection.Normalize();
        outDirection.Normalize();


        Vector3 outPerp = new Vector3(outDirection.y, -outDirection.x, outDirection.z);

        float perpForce = Vector3.Dot(outPerp, mouseDirection);

        perpForce = Mathf.Sign(perpForce) * swingSpeed;

        return outPerp * perpForce * physics2d.mass;
    }

}
