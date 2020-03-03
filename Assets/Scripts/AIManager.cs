using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private static AIManager _instance;
    public static AIManager Instance { get { return _instance; } }

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
    public List<MeleeAIAgent> enemies;
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

        foreach (MeleeAIAgent enemy in enemies)
        {
            enemy.ExecuteState();
        }

        
        int j = 0;
        LinkedListNode<MeleeAIAgent> enemyHead = enemyQueue.First;
        while (enemyHead != null && j < readyForEnemy.Count)
        {
            readyForEnemy.Sort((f1, f2) =>
                -Vector3.Distance(f1.transform.position, enemyHead.Value.transform.position).CompareTo(Vector3.Distance(f2.transform.position, enemyHead.Value.transform.position)));
            readyForEnemy[readyForEnemy.Count - 1].AttackEnemy(enemyHead.Value);
            readyForEnemy.RemoveAt(readyForEnemy.Count - 1);
            enemyQueue.RemoveFirst();
            enemyHead = enemyHead.Next;
            j += 1;
        }
        
    }

    /*
    public void ConvertToFriendly(NavAgent enemy)
    {
        friendlies.AddAfter(enemy);
    }
    */

    public void PutOnDispatchQueue(FriendlyMeleeAIAgent friendlyAI)
    {
        readyForEnemy.Add(friendlyAI);
    }

    public void PutOnEnemyQueue(MeleeAIAgent enemyAI)
    {
        enemyQueue.AddLast(enemyAI);
    }

    public void RemoveFromEnemyQueue(MeleeAIAgent enemyAI)
    {
        enemyQueue.Remove(enemyAI);
    }
}
