using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using System.Diagnostics;
using UnityEngine;

public class EnemyController : SlimeFighter
{
    EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {
        CommonStart();
        ai = GetComponentInChildren<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        CommonUpdate();

        GameObject enemy = GameObject.Find("Player");

        if (enemy == null || IsStunned)
        {
            return;
        }

        Vector2 toEnemy = enemy.transform.position - transform.position;
        float distance = toEnemy.magnitude;
        toEnemy.Normalize();

        Vector2 toWeapon = weapon.transform.position - transform.position;
        toWeapon.Normalize();

        switch (ai.currentGoal.goalType)
        {
            case AIGoalType.CHASE:
                if (distance < 2)
                {
                    ai.AchieveGoal();
                    return;
                }
                else
                {
                    Vector2 nextNode = ai.GetNextNode((Vector2)transform.position);
                    toEnemy = nextNode - (Vector2)transform.position;
                    toEnemy.Normalize();
                    physics2d.AddForce(speed * toEnemy * physics2d.mass);
                    weapon.physics2d.AddForce(speed * toEnemy * weapon.physics2d.mass);
                    return;
                }
            case AIGoalType.ATTACK:
                float angle = Vector2.Dot(toWeapon, toEnemy);
                if (angle > 0.95)
                {
                    weapon.Thrust();
                }
                else
                {
                    weapon.Swing(toEnemy, toWeapon);
                }
                ai.AchieveGoal();
                return;
            default:
                return;

        }

    }

   

    protected override bool WeaponIsEnemy(SlimeWeapon slimeWeapon)
    {
        return slimeWeapon is WeaponController;
    }

    protected override void DisplayHP()
    {
        gameObject.transform.localScale = CurrentScale * initialScale;
        actionHealthBar.SetValue(hp);
    }

}
