using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrector : MonoBehaviour
{
    public enum ResState
    {
        Idle,
        Preparing,
        Resurrecting
    }

    public float radius = 10.0f;
    private bool readyToRes = false;
    Coroutine resurrectionCoroutine;
    float resurrectHoldTime;
    ResState state;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        resurrectHoldTime = Time.time;
        state = ResState.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case ResState.Idle:
                if (Input.GetMouseButtonDown(1))
                {
                    GetComponent<MeshRenderer>().enabled = true;
                    resurrectHoldTime = Time.time + 1.0f;
                    resurrectionCoroutine = StartCoroutine(ResurrectionAnimation());
                    state = ResState.Preparing;
                }
                break;
            case ResState.Preparing:
                if(Time.time >= resurrectHoldTime)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                    ResurrectEnemies();
                    state = ResState.Resurrecting;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    state = ResState.Idle;
                    GetComponent<MeshRenderer>().enabled = false;
                    StopCoroutine(resurrectionCoroutine);
                }
                break;
            case ResState.Resurrecting:
                state = ResState.Idle;
                break;
        }
    }

    public void ResurrectEnemies()
    {
        int layerMask = 1 << 9;
        Collider[] collidersInRange = Physics.OverlapSphere(transform.parent.position, radius, layerMask);
        foreach (Collider enemyCollider in collidersInRange)
        {
            MeleeAIEnemy enemyAI = enemyCollider.gameObject.GetComponent<MeleeAIEnemy>();
            if (enemyAI != null && enemyAI.IsDead())
            {
                MeleeAIAlly allyAI = (MeleeAIAlly)Instantiate(Resources.Load("Prefabs/Ally", typeof(MeleeAIAlly)));
                allyAI.transform.position = enemyCollider.gameObject.transform.position;
                allyAI.transform.rotation = enemyCollider.gameObject.transform.rotation;
                allyAI.master = transform.parent;
                allyAI.target = transform.parent;
                Destroy(enemyCollider.gameObject);
                AIManager.Instance.allies.Add(allyAI);
                AIManager.Instance.agents.Add(allyAI);
            }
        }
    }

    /*
    public void PrepareForResurrection()
    {
        StopCoroutine(resurrectionCoroutine);
        
        resurrectionCoroutine = StartCoroutine(ResurrectionAnimation());
    }

    public void CancelResurrection()
    {
        StopCoroutine(resurrectionCoroutine);
    }
    */

    IEnumerator ResurrectionAnimation()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
