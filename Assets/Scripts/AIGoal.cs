using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public struct AIGoal
{
    public AIGoalType goalType;
    GameObject goalTarget;
    float goalTime;

    public bool hasExpired { get { return goalTime <= 0; } }
    public bool hasTarget { get { return goalTarget != null; } }

    public AIGoal(AIGoalType type, GameObject target, float time)
    {
        goalType = type;
        goalTarget = target;
        goalTime = time;
    }

    public void tick(float deltaTime)
    {
        goalTime -= deltaTime;
    }
}

public enum AIGoalType
{
    CHASE,
    FLEE,
    WAIT,
    COLLECT,
    ATTACK
}