using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionCircle : MonoBehaviour
{
    public float radius = 10f;

    public GameObject ally;

    private Transform playerTransform;
    
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
        Collider[] collidersInRange = Physics.OverlapSphere(playerTransform.position, radius);
        foreach (Collider enemyCollider in collidersInRange)
        {
            MeleeAIEnemy enemyAI = enemyCollider.gameObject.GetComponent<MeleeAIEnemy>();
            if (enemyAI != null && enemyAI.IsDead()){
                Object newAllyObject = Instantiate(ally, enemyCollider.gameObject.transform.position, enemyCollider.gameObject.transform.rotation);
                GameObject newAlly = (GameObject) newAllyObject;
                MeleeAIAlly newAllyAI = newAlly.GetComponent<MeleeAIAlly>();
                newAllyAI.master = playerTransform;
                newAllyAI.target = playerTransform;
                AIManager.Instance.enemies.Remove(enemyAI);
                AIManager.Instance.agents.Remove(enemyAI);
                Destroy(enemyCollider.gameObject);
                AIManager.Instance.allies.Add(newAllyAI);
                AIManager.Instance.agents.Add(newAllyAI);
            }
        }
    }
}
