using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionCircle : MonoBehaviour
{
    // FOR DEBUGGING
    private const bool DEBUG = true;

    // Radius around the player where resurrection can happen
    private float RESURRECTION_RADIUS = 10f;

    // Ally that will be resurrected. TODO: depreciate this and resurrect an ally that is the equivalent of the enemy being resurrected
    public GameObject ally;
    
    private bool readyToRes = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResurrectEnemies(Transform playerTransform){

        if (DEBUG) Debug.Log("attempting resurrection!");

        // Get all colliders within a certain radius sphere
        Collider[] collidersInRange = Physics.OverlapSphere(playerTransform.position, RESURRECTION_RADIUS);
        foreach (Collider enemyCollider in collidersInRange)
        {
            // Get enemy AI
            MeleeAIEnemy enemyAI = enemyCollider.gameObject.GetComponent<MeleeAIEnemy>();
            if (enemyAI != null && enemyAI.IsDead()){

                // Create a new ally
                GameObject newAlly = Instantiate(ally, enemyCollider.gameObject.transform.position, enemyCollider.gameObject.transform.rotation) as GameObject;
                
                // Get ally AI and set master and target to player transform
                MeleeAIAlly newAllyAI = newAlly.GetComponent<MeleeAIAlly>();
                newAllyAI.master = playerTransform;
                newAllyAI.target = playerTransform;

                // Remove enemy from AI manager
                AIManager.Instance.enemies.Remove(enemyAI);
                AIManager.Instance.agents.Remove(enemyAI);
                Destroy(enemyCollider.gameObject);

                // Add ally to AI manager
                AIManager.Instance.allies.Add(newAllyAI);
                AIManager.Instance.agents.Add(newAllyAI);
            }
        }
    }
}
