using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    private List<Collider> TriggerList = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(TriggerList.Count);
    }

    // Set rotation of hitbox
    public void SetRotation(float rot)
    {
        transform.eulerAngles = new Vector3(0, rot - 45, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HitDetector>()) // Hit detectors don't get damage or other things
        {
            return;
        }

        if (other.gameObject == transform.parent.gameObject) return;
        Debug.Log("entering:");
        Debug.Log(other);
        if (!TriggerList.Contains(other))
        {
            TriggerList.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HitDetector>())
        {
            return;
        }

        if (other.gameObject == transform.parent.gameObject) return;
        Debug.Log("exiting:");
        Debug.Log(other);
        if (TriggerList.Contains(other))
        {
            TriggerList.Remove(other);
        }
    }

    /**
     * This is to remove destroyed enemies from colliding
     */
    void CleanTriggerList()
    {
        List<Collider> newTriggerList = new List<Collider>();
        foreach (Collider c in TriggerList)
        {
            if (c)
            {
                newTriggerList.Add(c);
            }
        }

        TriggerList = newTriggerList;
    }

    public List<Collider> GetColliding()
    {
        CleanTriggerList();
        return TriggerList; // Don't modify it after being returned!
    }
}
