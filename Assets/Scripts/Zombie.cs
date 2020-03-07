using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Player player;
    private Rigidbody rb;
    private MovementManager moveManager;

    public float visionRadius;

    public float closeRadius;

    public float drag;

    public float separationStrength;

    public float matchStrength;

    public float groupStrength;

    public float homingStrength;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveManager = new BoidsMovement(this, rb);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        moveManager.MovementUpdate();
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}
