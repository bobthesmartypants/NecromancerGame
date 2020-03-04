﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavAgent : MonoBehaviour
{
    public List<Vector3> pathPoints;
    public int navMeshTriIdx;
    public Transform target;
    public float radius;
    public string id;
    protected readonly float TARGET_SPEED = 10.0f;
    public List<HalfPlane> ORCAHalfPlanes;
    public Vector3 desiredHeading;


    // Start is called before the first frame update
    protected void Start()
    {
        desiredHeading = TARGET_SPEED * (target.position - transform.position).normalized;
        Vector3 spriteBounds = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().bounds.size;
        radius = spriteBounds.x / 2;
        transform.forward = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.up;
        pathPoints = new List<Vector3>();
    }

    public virtual void MoveAgent(Vector3 heading)
    {

    }
}

