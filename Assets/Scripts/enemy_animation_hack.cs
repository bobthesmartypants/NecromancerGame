using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_animation_hack : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody rb;
    void Start()
    {
        GameObject sprite = transform.Find("goomba").gameObject;
        rb = GetComponent<Rigidbody>();
        animator = sprite.GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = rb.velocity;
        animator.SetFloat("movement", movement.magnitude);
        Debug.Log(movement);
        animator.SetFloat("facingY", movement.z);
        animator.SetFloat("facingX", movement.x);
    }
}
