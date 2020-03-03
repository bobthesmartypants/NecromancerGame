using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAIAgent : NavAgent
{
    public enum AIState
    {
        Attacking,
        NavigatingToPlayer
    }
    AIState state;
    public Transform playerTrans;
    float ATTACK_RADIUS = 30.0f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        state = AIState.NavigatingToPlayer;
    }

    public void ExecuteState()
    {
        //Attack player
        switch (state)
        {
            case AIState.Attacking:
                //Attack closest of the friendlies and player
                Transform closestAgent = playerTrans;
                foreach(FriendlyMeleeAIAgent friendly in FriendlyAIManager.Instance.friendlies)
                {
                    if(Vector3.Distance(friendly.transform.position, transform.position) < Vector3.Distance(closestAgent.position, transform.position))
                    {
                        closestAgent = friendly.transform;
                    }
                }
                target = closestAgent;

                if ((playerTrans.position - transform.position).magnitude >= ATTACK_RADIUS)
                {
                    state = AIState.NavigatingToPlayer;
                    target = playerTrans;
                    FriendlyAIManager.Instance.RemoveFromEnemyQueue(this);
                }
                break;
            case AIState.NavigatingToPlayer:
                if ((playerTrans.position - transform.position).magnitude < ATTACK_RADIUS)
                {
                    state = AIState.Attacking;
                    FriendlyAIManager.Instance.PutOnEnemyQueue(this);
                }
                break;
        }
    }


    public override void MoveAgent(Vector3 heading)
    {
        transform.Translate(heading * Time.deltaTime, Space.World);
        Vector3 curPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        //desiredHeading = TARGET_SPEED * (pathPoints[0] - curPos).normalized;
        //Smooth movement
        desiredHeading = Vector3.Lerp(heading, TARGET_SPEED * (pathPoints[0] - curPos).normalized, 5.0f * Time.deltaTime);
        Debug.DrawLine(curPos, curPos + desiredHeading, Color.magenta);

    }
}
