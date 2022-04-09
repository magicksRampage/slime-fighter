using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyAIBasic : EnemyAI
{
    public int visibleDistance = 10;
    public Pathfinder pathfinder;

    Vector2[] currentPath;
    int targetIndex;
    float timeToPathRefresh;

    // Start is called before the first frame update
    void Start()
    {
        CommonStart();
    }

    // Update is called once per frame
    void Update()
    {
        CommonUpdate();

    }

    protected override void SetInitialGoal()
    {
        currentGoal = new AIGoal(AIGoalType.WAIT, body.gameObject, 3.0f);
    }

    protected override void SetNextGoal()
    {
        GameObject enemy = GameObject.Find("Player");

        if (enemy == null)
        {
            SetInitialGoal();
            return;
        }

        currentPath = pathfinder.FindPath(body.transform.position, enemy.transform.position);
        targetIndex = 0;

        float distance = (enemy.transform.position - body.transform.position).magnitude;
        if (distance > visibleDistance)
        {
            currentGoal = new AIGoal(AIGoalType.WAIT, body.gameObject, (distance - visibleDistance) * 0.2f);
            return;
        }

        switch (currentGoal.goalType)
        {
            case AIGoalType.WAIT:
                currentGoal = new AIGoal(AIGoalType.CHASE, enemy, 0.25f);
                return;
            case AIGoalType.CHASE:
                currentGoal = new AIGoal(AIGoalType.CHASE, enemy, 0.25f);
                return;
            case AIGoalType.ATTACK:
                currentGoal = new AIGoal(AIGoalType.WAIT, body.gameObject, 1.0f);
                return;
            default:
                currentGoal = new AIGoal(AIGoalType.CHASE, enemy, 0.25f);
                return;
        }
    }

    public override Vector2 GetNextNode(Vector2 currentPosition)
    {
        while (currentPath[targetIndex] == currentPosition)
        {
            targetIndex++;
        }
        return currentPath[targetIndex];
    }

    public override void AchieveGoal()
    {
        GameObject enemy = GameObject.Find("Player");

        if (enemy == null)
        {
            SetInitialGoal();
            return;
        }
        else
        {
            float distance = (enemy.transform.position - body.transform.position).magnitude;
            if (distance > visibleDistance)
            {
                currentGoal = new AIGoal(AIGoalType.WAIT, body.gameObject, (distance - visibleDistance) * 0.2f);
                return;
            }
        }
        switch (currentGoal.goalType)
        {
            case AIGoalType.CHASE:
                currentGoal = new AIGoal(AIGoalType.ATTACK, enemy, 2.0f);
                return;
            default:
                currentGoal = new AIGoal(AIGoalType.WAIT, body.gameObject, 2.0f);
                return;
        }


    }
}
