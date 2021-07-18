using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public AIGoal currentGoal;
    protected EnemyController body;

    // Start is called before the first frame update
    protected void CommonStart()
    {
        body = GetComponentInParent<EnemyController>();
        SetInitialGoal();
    }

    // Update is called once per frame
    protected void CommonUpdate()
    {
        currentGoal.tick(Time.deltaTime);
        if (currentGoal.hasExpired || !currentGoal.hasTarget)
        {
            SetNextGoal();
        }
    }

    protected abstract void SetInitialGoal();

    protected abstract void SetNextGoal();

    public abstract void AchieveGoal();

    public abstract Vector2 GetNextNode(Vector2 currentPosition);
}


