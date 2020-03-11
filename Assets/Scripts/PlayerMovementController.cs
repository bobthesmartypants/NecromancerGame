using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementController : MonoBehaviour
{
    // FOR DEBUGGING
    private const bool DEBUG = true;

    #region Constants
    private const float HAND_DISTANCE = 1.4f; // Distance of player's hand from player's body
    private const float HAND_HEIGHT = 1.5f; // Height of player's hand from ground
    private const float ATTACK_RECHARGE = 1f; // Time it takes for player to recharge its attack
    private const float PLAYER_SPEED = 15.0f; // Player running speed
    private const float HITBOX_SIZE = 5f; // Attack hitbox radius
    private const float HITBOX_HEIGHT = 10f; // Attack hitbox vertical height (need to be set bigger for dealing with big enemies perhaps)
    #endregion

    #region Private Variables
    // Rigidbody reference
    private Rigidbody rb;

    // Animator references
    private Animator animator;
    private Animator AttackAnim;

    // Boolean for checking whether player can currently attack
    private bool canAttack;

    // Reference to ResurrectionCircle script
    private ResurrectionCircle resCircle;

    // Reference to HitDetector script
    private HitDetector hitbox;

    // Current horizontal and vertical input
    private float moveHorizontal;
    private float moveVertical;
    // Mouse position relative to player position
    private Vector3 relMousePos;

    // Reference to transform of Hand GameObject
    private Transform hand;

    #endregion

    // Note sure if this is still useful soon
    Magic equippedMagic;

    #region Public Variables
    public float radius; // Radius of player. TODO: use C# getters and setters instead

    public Vector3 velocity; // Velocity of player. TODO: use C# getters and setters instead

    public Sword sword; // Reference to Sword script
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Initialize Rigidbody reference
        rb = GetComponent<Rigidbody>();

        // Initialize sprite animator
        GameObject sprite = transform.Find("sprite").gameObject;
        animator = sprite.GetComponent<Animator>();

        // Initialize radius of player
        Vector3 spriteBounds = sprite.GetComponent<SpriteRenderer>().bounds.size;
        radius = spriteBounds.x / 2;

        // Initialize magic
        equippedMagic = gameObject.AddComponent<Fire>();

        // Point player sprite in same direction as camera
        transform.forward = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.up;

        // Initialize sword and place it in hand
        hand = transform.Find("Hand");
        sword.transform.parent = hand;
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        // Initialize attack animation
        AttackAnim = sword.GetComponent<Animator>();

        // Initialize resurrection circle reference
        resCircle = gameObject.GetComponentInChildren<ResurrectionCircle>();

        // Initiaize hit detector reference
        hitbox = gameObject.GetComponentInChildren<HitDetector>();

        // Initialize hit detector range
        hitbox.transform.localScale = new Vector3(HITBOX_SIZE, HITBOX_HEIGHT, HITBOX_SIZE);

        // Set initial attack state to true
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If left button clicked and currently can attack
        if (Input.GetMouseButton(0) && canAttack)
        {
            // equippedMagic.Cast(); // Cast the current magic TODO: make this obsolete?

            DoAttack();

            AttackAnim.SetTrigger("Attack"); // Do attack animation
            canAttack = false; // Disable attack
            StartCoroutine("RechargeAttack"); // Reenable attack after recharged
        }

        // If right mouse button clicked
        if (Input.GetMouseButton(1)) 
        {
            // Resurrect all enemies within the resurrection circle
            resCircle.ResurrectEnemies(transform);
        }

    }

    private void DoAttack()
    {
        if (DEBUG) Debug.Log("Doing attack");

        foreach (Collider other in hitbox.GetColliding())
        {
            if (DEBUG) Debug.Log("Found entity in collider");
            
            if (other.gameObject.GetComponent<MeleeAIEnemy>())
            {
                if (DEBUG) Debug.Log("enemy taking damage");
                MeleeAIEnemy enemyAI = other.gameObject.GetComponent<MeleeAIEnemy>();
                
                enemyAI.TakeDamage(1); 
            }
        }
    }
    
    // Sets canAttack to true after ATTACK_RECHARGE seconds
    IEnumerator RechargeAttack()
    {
        yield return new WaitForSeconds(ATTACK_RECHARGE);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        // Get input from movement and get relative mouse position to player position
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        relMousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        relMousePos = new Vector3(relMousePos.x, relMousePos.y, 0.0f).normalized;

        // Set hand position
        hand.transform.position = transform.position + new Vector3(relMousePos.x, HAND_HEIGHT, relMousePos.y) * HAND_DISTANCE;

        // Set sword rotation
        sword.SetRotation(Vector3.SignedAngle(relMousePos, Vector3.right, Vector3.forward));

        // Set hitbox rotation
        hitbox.SetRotation(Vector3.SignedAngle(relMousePos, Vector3.right, Vector3.forward));

        // Calculate movement vector
        velocity = Vector3.zero;
        Vector3 movement = moveVertical * Vector3.forward + moveHorizontal * Vector3.right;
        if (movement.magnitude > 1.0f) // Normalize movement vector
        {
            movement = movement.normalized;
        }

        // Animate player sprite based on inputs
        animator.SetFloat("movement", movement.magnitude);
        animator.SetFloat("facingY", relMousePos.y);
        animator.SetFloat("facingX", relMousePos.x);

        // Actually move player if movement is big enough
        if (movement.magnitude > 0.25f)
        {
            Vector3.Normalize(movement);

            // Pass in relevant information for running animation 
            animator.SetFloat("dx", movement.x);
            animator.SetFloat("dy", movement.z);

            // Set new velocity
            velocity = movement.normalized * PLAYER_SPEED;

            // Actually move player
            transform.Translate(movement.normalized * PLAYER_SPEED * Time.fixedDeltaTime, Space.World);
        }

    }

}

