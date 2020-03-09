using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementController : MonoBehaviour
{

    private const float HAND_DISTANCE = 1.4f;
    private const float HAND_HEIGHT = 1.5f;
    private const float ATTACK_RECHARGE = 1f;

    private bool canAttack;

    private Animator AttackAnim;

    public float radius;

    /*
    public Vector3 velocity
    {
        get { return rb.velocity; }
    }
    */

    public Vector3 velocity;

    float playerSpeed = 15.0f;
    Magic equipedMagic;
    public Sword sword;

    float moveHorizontal;
    float moveVertical;
    Vector3 relMousePos;

    Animator animator;
    Rigidbody rb;

    Transform hand;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject sprite = transform.Find("sprite").gameObject;
        animator = sprite.GetComponent<Animator>();

        Vector3 spriteBounds = sprite.GetComponent<SpriteRenderer>().bounds.size;
        radius = spriteBounds.x / 2;

        equipedMagic = gameObject.AddComponent<Fire>();
        transform.forward = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.up;

        //Attach sword to hand
        hand = transform.Find("Hand");
        sword.transform.parent = hand;
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        AttackAnim = sword.GetComponent<Animator>();

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0) && canAttack)
        {
            equipedMagic.Cast();
            AttackAnim.SetTrigger("Attack");
            canAttack = false;
            StartCoroutine("RechargeAttack");
        }

    }

    IEnumerator RechargeAttack()
    {
        yield return new WaitForSeconds(ATTACK_RECHARGE);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        relMousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        relMousePos = new Vector3(relMousePos.x, relMousePos.y, 0.0f).normalized;

        // Set hand position
        hand.transform.position = transform.position + new Vector3(relMousePos.x, HAND_HEIGHT, relMousePos.y) * HAND_DISTANCE;

        // Set sword rotation
        sword.SetRotation(Vector3.SignedAngle(relMousePos, Vector3.right, Vector3.forward));

        velocity = Vector3.zero;
        Vector3 movement = moveVertical * new Vector3(0.0f, 0.0f, 1.0f) + moveHorizontal * new Vector3(1.0f, 0.0f, 0.0f);
        if (movement.magnitude > 1.0f)
        {
            movement = movement.normalized;
        }
        animator.SetFloat("movement", movement.magnitude);
        animator.SetFloat("facingY", relMousePos.y);
        animator.SetFloat("facingX", relMousePos.x);

        if (movement.magnitude > 0.25f)
        {
            Vector3.Normalize(movement);
            animator.SetFloat("dy", movement.z);
            animator.SetFloat("dx", movement.x);
            velocity = movement.normalized * playerSpeed;
            transform.Translate(movement.normalized * playerSpeed * Time.fixedDeltaTime, Space.World);
        }

    }

}

