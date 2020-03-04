using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FriendlyMeleeAIAgent : NavAgent
{
    public enum AIState
    {
        AttackingEnemy,
        ReturningToMaster,
        SearchingForEnemy
    }
    AIState state;
    public Transform master;
    Transform wanderingTransform;
    MeleeAIAgent enemyTarget;
    float nextWanderTime;
    float WANDER_RADIUS = 30.0f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        nextWanderTime = Time.time;
        state = AIState.ReturningToMaster;

        GameObject go = new GameObject();
        go.transform.position = transform.position;
        wanderingTransform = go.transform;

        target = master;
    }

    public void ExecuteState()
    {
        
        switch (state)
        {
            case AIState.AttackingEnemy:
                if ((master.position - transform.position).magnitude >= WANDER_RADIUS)
                {
                    state = AIState.ReturningToMaster;
                    target = master;
                    //Here an enemy can potentially have no pursuers
                    enemyTarget.RemovePursuer(this);
                    enemyTarget = null;
                }
                break;
            case AIState.ReturningToMaster:
                if ((master.position - transform.position).magnitude < WANDER_RADIUS)
                {
                    state = AIState.SearchingForEnemy;
                    target = wanderingTransform;
                }
                break;
            case AIState.SearchingForEnemy:
                if ((master.position - transform.position).magnitude >= WANDER_RADIUS)
                {
                    state = AIState.ReturningToMaster;
                    target = master;
                }
                else
                {
                    AIManager.Instance.PutOnDispatchQueue(this);
                    if (Time.time > nextWanderTime)
                    {
                        nextWanderTime = Time.time + Random.Range(3.0f, 8.0f);
                        Vector2 randPos2d = WANDER_RADIUS * Random.insideUnitCircle;
                        target.position = master.position + new Vector3(randPos2d.x, 0.0f, randPos2d.y);
                    }
                }
                break;
        }
    }

    public void AttackEnemy(MeleeAIAgent enemyAI)
    {
        state = AIState.AttackingEnemy;
        enemyTarget = enemyAI;
        target = enemyAI.transform;
    }

    public void TargetWasKilled()
    {
        state = AIState.SearchingForEnemy;
        enemyTarget = null;
        target = wanderingTransform;
    }

    public override void MoveAgent(Vector3 heading)
    {
        transform.Translate(heading * Time.deltaTime, Space.World);
        Vector3 curPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        //desiredHeading = TARGET_SPEED * (pathPoints[0] - curPos).normalized;
        //Smooth movement
        desiredHeading = Vector3.Lerp(heading, TARGET_SPEED * (pathPoints[0] - curPos).normalized, 5.0f * Time.deltaTime);
        Debug.DrawLine(curPos, curPos + desiredHeading, Color.cyan);
    }

    private void OnDestroy()
    {
        //TODO: Remove from NavMesh dictionary

        if (wanderingTransform)
        {
            Destroy(wanderingTransform.gameObject);
        }
        
    }

}
