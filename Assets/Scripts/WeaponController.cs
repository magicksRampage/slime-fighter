using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponController : SlimeWeapon
{

    Camera cam;
    
    public int staminaCostSwing = 1;
    public int staminaCostThrust = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
        CommonStart();
        
        cam = Camera.main;
    }


    void Update()
    {

        CommonUpdate();

        PlayerController player = GetComponentInParent<PlayerController>();

        if (player.stamina > staminaCostThrust && Input.GetKeyDown(KeyCode.Mouse1) && !player.IsStunned)
        {
            timeThrust = 1.0f;

            player.stamina -= staminaCostThrust;
            player.staminaBar.SetValue((int)Mathf.Floor(player.stamina));

        }

        goalDistance = initialDistance + timeThrust;

    }


    void FixedUpdate()
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

        float perpForce = Vector3.Dot(outPerp,mouseDirection);

        perpForce = Mathf.Sign(perpForce) * swingSpeed;


        
        if (!player.IsStunned)
        {
            physics2d.AddForce(outPerp * perpForce * physics2d.mass);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.IsStunned && player.stamina > staminaCostSwing)
        {
            physics2d.AddForce(activeSwing * outPerp * perpForce * physics2d.mass, ForceMode2D.Impulse);

            player.stamina -= staminaCostSwing;
            player.staminaBar.SetValue((int)Mathf.Floor(player.stamina));
        }



        Orbit();


    }

}
