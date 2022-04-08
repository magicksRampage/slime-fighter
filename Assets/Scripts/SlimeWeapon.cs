using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeWeapon : MonoBehaviour
{
    public Rigidbody2D physics2d;
    public float initialDistance = 0.4f;

    public float speed = 5.0f;
    public float swingSpeed = 1.0f;
    public float activeSwing = 0.5f;
    public float thrustSpeed = 1.0f;
    public float centralForce = 1.0f;
    public float centralDampening = 1.0f;
    public int baseDamage = 1;
    public AudioClip audioSwing;

    protected float timeThrust = 0;
    protected float goalDistance;

    protected SlimeFighter wielder;
    protected AudioSource audioSource;

    // Start is called before the first frame update
    protected void CommonStart()
    {
        physics2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        goalDistance = initialDistance;

        wielder = GetComponentInParent<SlimeFighter>();
    }

    // Update is called once per frame
    protected void CommonUpdate()
    {
        if (timeThrust > 0)
        {
            timeThrust -= Time.deltaTime;
        }

        if (timeThrust < 0)
        {
            timeThrust = 0;
        }
    }

    protected void Orbit()
    {

        Quaternion rotation = transform.rotation;
        Vector3 position = transform.position;

        Vector3 comPosition = transform.parent.position;
        Vector3 outDirection = position - comPosition;

        outDirection.Normalize();

        float outVelocity = Vector3.Dot(outDirection, physics2d.velocity);


        physics2d.AddForce(centralForce * (comPosition + outDirection * goalDistance * wielder.CurrentScale - position) * physics2d.mass);
        physics2d.AddForce(-centralDampening * outVelocity * outDirection * physics2d.mass);

        rotation = Quaternion.LookRotation(outDirection, -Vector3.forward);

        physics2d.MoveRotation(rotation);
    }

    public void Thrust()
    {
        timeThrust = 1.0f;
        audioSource.PlayOneShot(audioSwing);

    }

    public void Swing(Vector2 targetDirection, Vector2 outDirection)
    {

        targetDirection.Normalize();
        outDirection.Normalize();


        Vector2 outPerp = new Vector2(outDirection.y, -outDirection.x);

        float perpForce = Vector2.Dot(outPerp, targetDirection);

        perpForce = Mathf.Sign(perpForce) * swingSpeed;


        physics2d.AddForce(activeSwing * outPerp * perpForce * physics2d.mass, ForceMode2D.Impulse);
        audioSource.PlayOneShot(audioSwing);
    }
}
