using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAIManager : MonoBehaviour
{
    private static FriendlyAIManager _instance;
    public static FriendlyAIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public List<FriendlyMeleeAIAgent> friendlies;
    LinkedList<MeleeAIAgent> enemyQueue;
    List<FriendlyMeleeAIAgent> readyForEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyQueue = new LinkedList<MeleeAIAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        readyForEnemy = new List<FriendlyMeleeAIAgent>();
        foreach (FriendlyMeleeAIAgent friendly in friendlies)
        {
            friendly.ExecuteState();
        }

        LinkedListNode<MeleeAIAgent> head = enemyQueue.First;
        if (head != null)
        {
            readyForEnemy.Sort((f1, f2) => 
            Vector3.Distance(f1.transform.position, head.Value.transform.position).CompareTo(Vector3.Distance(f2.transform.position, head.Value.transform.position)));

            if (readyForEnemy.Count > 0)
            {
                readyForEnemy[0].AttackEnemy(head.Value);
            }
        }
    }

    /*
    public void ConvertToFriendly(NavAgent enemy)
    {
        friendlies.AddAfter(enemy);
    }
    */

    public void PutOnQueue(FriendlyMeleeAIAgent friendlyAI)
    {
        readyForEnemy.Add(friendlyAI);
    }
    


    private void OnTriggerEnter(Collider other)
    {
        MeleeAIAgent enemyAI = other.gameObject.GetComponent<MeleeAIAgent>();
        if (enemyAI)
        {
            enemyQueue.AddLast(enemyAI);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        MeleeAIAgent enemyAI = other.gameObject.GetComponent<MeleeAIAgent>();
        if (enemyAI)
        {
            enemyQueue.Remove(enemyAI);
        }
        
    }

}
