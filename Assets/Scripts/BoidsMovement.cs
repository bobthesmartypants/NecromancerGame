using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BoidsMovement: MovementManager
{
    private Zombie agent;
    private Rigidbody rb;
    private float visionRadius;
    private float closeRadius;
    private float drag;
    private float separationStrength;
    private float matchStrength;
    private float groupStrength;
    private float homingStrength;
    private float speed;

    public BoidsMovement(Zombie mine, Rigidbody myRb)
    {
        agent = mine;
        rb = myRb;
        visionRadius = agent.visionRadius;
        closeRadius = agent.closeRadius;
        drag = agent.drag;
        separationStrength = agent.separationStrength;
        matchStrength = agent.matchStrength;
        groupStrength = agent.groupStrength;
        homingStrength = agent.homingStrength;
        speed = agent.speed;
    }
    override public void MovementUpdate()
    {
        Transform transform = agent.transform;
        Collider[] closeColliders = Physics.OverlapSphere(transform.position, visionRadius);
        List<Zombie> hordemates = new List<Zombie>();
        for (int i = 0; i < closeColliders.Length; i++)
        {
            Collider c = closeColliders[i];
            Zombie z = c.gameObject.GetComponent<Zombie>();
            if (z && z != agent) hordemates.Add(z);
        }

//        if (hordemates.Count == 0) // TODO: fix so that will go to player if haven't got mates
//        {
//            return;
//        }
        int totClose = 0;
        Vector3 closeDir = new Vector3();
        Vector3 totPos = new Vector3();
        Vector3 totVel = new Vector3();
        foreach (Zombie hordemate in hordemates)
        {
            if (Vector3.Distance(transform.position, hordemate.transform.position) < closeRadius)
            {
                totClose++;
                float dist = Vector3.Distance(transform.position, hordemate.transform.position);
                closeDir += (transform.position - hordemate.transform.position) /
                            (dist * dist);
            }

            totPos += hordemate.transform.position - transform.position;
            totVel += hordemate.GetVelocity();
        }
        if (totVel.magnitude > 0) totVel /= totVel.magnitude;
        if (totClose > 0) closeDir /= totClose;
        Vector3 currVelocityDir = rb.velocity;
        if (rb.velocity.magnitude > 0) currVelocityDir /= currVelocityDir.magnitude;
        totPos /= totPos.magnitude;
        Vector3 playerDir = agent.player.transform.position - transform.position;
        playerDir /= playerDir.magnitude;
        //
        //        Debug.Log("things:");
        //        Debug.Log(totVel);
        //        Debug.Log(totClose);
        //        Debug.Log(currVelocityDir);
        //        Debug.Log(totPos);
        //        Debug.Log(closeDir);
        Vector3 force = -drag * rb.velocity * (rb.velocity.magnitude - speed) +
                        separationStrength * closeDir +
                        matchStrength * (totVel - currVelocityDir) +
                        groupStrength * totPos +
                        homingStrength * playerDir;
        rb.AddForce(force);
    }
}