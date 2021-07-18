using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnemyWeaponController : SlimeWeapon
{
    
    // Start is called before the first frame update
    void Start()
    {

        CommonStart();
    }


    void Update()
    {

        CommonUpdate();

        EnemyController enemy = GetComponentInParent<EnemyController>();


        if (!enemy.IsStunned)
        {
            if (false)
            {
                Thrust();

            }
        }
        

    }

    void LateUpdate()
    {
        goalDistance = initialDistance + timeThrust;
    }


    void FixedUpdate()
    {
        EnemyController enemy = GetComponentInParent<EnemyController>();

        Vector3 position = transform.position;

        Vector3 comPosition = transform.parent.position;
        Vector3 outDirection = position - comPosition;

        outDirection.Normalize();


        Vector3 outPerp = new Vector3(outDirection.y, -outDirection.x, outDirection.z);

        Vector3 mouseDirection = outPerp;

        float perpForce = Vector3.Dot(outPerp, mouseDirection);

        perpForce = Mathf.Sign(perpForce) * swingSpeed;

        if (!enemy.IsStunned)
        {
            if (false)
            {
                physics2d.AddForce(outPerp * perpForce * physics2d.mass);
            }

            if (false)
            {
                physics2d.AddForce(activeSwing * outPerp * perpForce * physics2d.mass, ForceMode2D.Impulse);
            }
        }

        Orbit();
    }

}
