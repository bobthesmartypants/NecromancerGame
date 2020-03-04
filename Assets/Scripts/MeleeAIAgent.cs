using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAIAgent : NavAgent
{
    public enum AIState
    {
        Attacking,
        NavigatingToPlayer,
        Dying
    }
    AIState state;
    public Transform playerTrans;
    float ATTACK_RADIUS = 30.0f;
    float health = 3.0f;

    //List<FriendlyMeleeAIAgent> pursuers;
    List<FriendlyMeleeAIAgent> pursuers = new List<FriendlyMeleeAIAgent>();

    //How much more the enemies prefer attacking the player over allies
    static float PLAYER_PREFERENCE = 2.0f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        speed = 12.0f;
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
                float minDistance = Vector3.Distance(closestAgent.position, transform.position) / PLAYER_PREFERENCE;
                foreach(FriendlyMeleeAIAgent friendly in AIManager.Instance.friendlies)
                {
                    float dist = Vector3.Distance(friendly.transform.position, transform.position);
                    if (dist < minDistance)
                    {
                        closestAgent = friendly.transform;
                        minDistance = dist;
                    }
                }
                target = closestAgent;

                if ((playerTrans.position - transform.position).magnitude >= ATTACK_RADIUS)
                {
                    state = AIState.NavigatingToPlayer;
                    target = playerTrans;
                    
                }
                //If enemy does not have any pursuers, put it on the queue so that it can be assigned some
                else if(pursuers.Count == 0)
                {
                    AIManager.Instance.PutOnEnemyQueue(this);
                }
                break;
            case AIState.NavigatingToPlayer:
                if ((playerTrans.position - transform.position).magnitude < ATTACK_RADIUS)
                {
                    state = AIState.Attacking;
                }
                break;
            case AIState.Dying:
                //Remove from navmesh agents
                AIManager.Instance.agents.Remove(this);
                AIManager.Instance.enemies.Remove(this);
                //Play dying animation with coroutine maybe
                Destroy(this.gameObject, 0.5f);
                break;
        }
    }


    public override void MoveAgent(Vector3 heading)
    {
        transform.Translate(heading * Time.deltaTime, Space.World);
        Vector3 curPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        //desiredHeading = TARGET_SPEED * (pathPoints[0] - curPos).normalized;
        //Smooth movement
        desiredHeading = Vector3.Lerp(heading, speed * (pathPoints[0] - curPos).normalized, 5.0f * Time.deltaTime);
        Debug.DrawLine(curPos, curPos + desiredHeading, Color.cyan);

    }

    public void AddPursuer(FriendlyMeleeAIAgent friendly)
    {
        pursuers.Add(friendly);
    }

    public void RemovePursuer(FriendlyMeleeAIAgent friendly)
    {
        pursuers.Remove(friendly);
    }

    private void OnDestroy()
    {
        //TODO: Remove from NavMesh dictionary
    }

    //This gets called before Update functions.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<FriendlyMeleeAIAgent>())
        {
            health -= Time.deltaTime;
            if (health < 0)
            {
                //ERROR. Not necessarily killed by friendly that targeted it
                state = AIState.Dying;

                //Disable collider to avoid future triggers
                gameObject.GetComponent<Collider>().enabled = false;
                foreach (FriendlyMeleeAIAgent friendly in pursuers)
                {
                    friendly.TargetWasKilled();
                }
            }
        }
    }
}
