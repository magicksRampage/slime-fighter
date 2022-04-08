using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public abstract class SlimeFighter : MonoBehaviour
{

    public float speed = 5.0f;
    public float dashSpeed = 10.0f;
    public int maxHp = 100;
    public int maxStamina = 5;

    public float stamina;
    public float hp;
    protected float stunned = 0.0f;
    protected float invincible = 0.0f;

    public bool IsInvincible { get { return invincible > 0; } }
    public bool IsStunned { get { return stunned > 0; } }

    protected Color baseColor;
    public Color invColor;

    protected Rigidbody2D physics2d;
    protected SpriteRenderer renderer;
    protected AudioSource audioSource;

    public SlimeWeapon weapon;

    public GameObject hpDrop;
    public GameObject splatterEffect;

    protected Vector2 initialScale;
    public float CurrentScale { get { return 1.0f; } }

    public ActionBar actionHealthBar;
    public ActionBar actionStaminaBar;

    public AudioClip clipSplat;

    // Start is called before the first frame update
    protected void CommonStart()
    {
        physics2d = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        stamina = maxStamina;

        hp = maxHp;

        baseColor = renderer.color;

        initialScale = gameObject.transform.localScale;

        if (actionHealthBar)
        {
            actionHealthBar.SetColor(baseColor);
            actionHealthBar.SetMaxValue(maxHp);
            actionHealthBar.SetValue(hp);
        }

        if (actionStaminaBar)
        {
            actionStaminaBar.SetColor(new Color(0, 0.5f, 0, 1));
            actionStaminaBar.SetMaxValue(maxStamina);
            actionStaminaBar.SetValue(stamina);
        }
        
    }

    // Update is called once per frame
    protected void CommonUpdate()
    {
        if (stunned > 0)
        {
            stunned -= Time.deltaTime;
        }

        if (stunned < 0)
        {
            stunned = 0;
        }

        if (invincible > 0)
        {
            invincible -= Time.deltaTime;
        }

        if (invincible < 0)
        {
            invincible = 0;
        }

        if (IsInvincible)
        {
            renderer.color = invColor;
        }
        else
        {
            renderer.color = baseColor;
        }

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        
        if (stamina < 0)
        {
            stamina = 0;
        }

        if (hp > maxHp)
        {
            float overflow = hp - maxHp;

            hp = maxHp + overflow * (float)Math.Pow(0.5f, Time.deltaTime / 5);
            DisplayHP();
        }
    }

    public void Heal(int amount)
    {
        hp += amount;
        stamina += amount / 100.0f;


        DisplayHP();
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        SlimeWeapon enemyWeapon = other.gameObject.GetComponent<SlimeWeapon>();

        if (WeaponIsEnemy(enemyWeapon) && !IsInvincible)
        {
            ContactPoint2D contact = other.GetContact(0);
            Vector2 weaponVelocity = contact.relativeVelocity;
            int damage = (int)Mathf.Round(enemyWeapon.baseDamage * weaponVelocity.magnitude);

            Damage(damage);

            Vector2 knockback = transform.position - other.transform.position;
            knockback.Normalize();

            physics2d.AddForce(0.5f * dashSpeed * knockback * physics2d.mass, ForceMode2D.Impulse);
            weapon.physics2d.AddForce(0.5f * dashSpeed * knockback * weapon.physics2d.mass, ForceMode2D.Impulse);

            float drop;
            while (damage > 0)
            {
                int bias = 2;
                drop = 0.0f;
                for (int i = 0; i < bias; i++)
                {
                    drop += UnityEngine.Random.value * damage / bias;
                }

                drop = Mathf.Round(drop);

                if (drop > 0.0f)
                {
                    GameObject dropObject = Instantiate(hpDrop, contact.point, Quaternion.identity);

                    HPDrop dropScript = dropObject.GetComponent<HPDrop>();
                    dropScript.SetValue((int)drop / 2);
                    dropScript.SetColor(renderer.color);
                    Vector2 direction = weaponVelocity + UnityEngine.Random.insideUnitCircle * 5;
                    direction.Normalize();
                    dropScript.Launch(weaponVelocity.magnitude * UnityEngine.Random.Range(0.8f, 1.2f), direction);
                }
                damage -= (int)drop;
            }

            GameObject splatterObject = Instantiate(splatterEffect, contact.point, Quaternion.FromToRotation(Vector3.up, new Vector3(contact.point.x, contact.point.y, transform.position.z) - transform.position));

            SplatterEffect splatterScript = splatterObject.GetComponent<SplatterEffect>();
            splatterScript.SetColor(renderer.color);
            audioSource.PlayOneShot(clipSplat);
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        DisplayHP();
        if (hp <= 0)
        {
            Destroy(gameObject);
        }


        stunned = 0.5f;
        invincible = 1.0f;
    }

    abstract protected bool WeaponIsEnemy(SlimeWeapon slimeWeapon);

    abstract protected void DisplayHP();

    protected void Move(Vector2 move)
    {
        move.Normalize();

        if (!IsStunned)
        {
            physics2d.AddForce(speed * move * physics2d.mass);
            weapon.physics2d.AddForce(speed * move * weapon.physics2d.mass);
        }
    }
    
}
