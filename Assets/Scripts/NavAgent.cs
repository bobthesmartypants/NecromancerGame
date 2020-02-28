using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    public List<Vector3> pathPoints;
    public int navMeshTriIdx;
    public Transform target;
    public float radius;
    public string id;
    private readonly float TARGET_SPEED = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 meshBounds = GetComponent<Renderer>().bounds.size;
        radius = meshBounds.x / 2;
        transform.forward = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine where agent will head to
        
        Vector3 heading = TARGET_SPEED * (pathPoints[1] - transform.position).normalized;
        transform.Translate(heading * Time.deltaTime, Space.World);

    }
}
