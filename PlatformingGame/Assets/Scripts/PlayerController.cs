using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Blackboard blackboard;

    Animator animator;

    [SerializeField]
    float height = 2;
    [SerializeField]
    float playerRadius = 0.25f;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("How fast the player moves")]
    [Range(0, 50)]
    float moveAcceleration = 5.0f;
    [SerializeField]
    float maxSpeed = 10.0f;
    float moveVelocity = 0.0f;
    [SerializeField]
    [Range(0, 100)]
    float moveDeceleration = 5.0f;

    [Header("Animation Stuff")]
    [SerializeField]
    float punchAnimationTime = 0.533f;
    float punchAnimationCountdown = 0;

    [SerializeField]
    float idleTime = 1;
    float idleCountdown = 0;
    
    [SerializeField]
    float innitialJumpVelocity = 1.0f;
    [SerializeField]
    float jumpDeceleration = 1.0f;
    float jumpVelocity = 0.0f;
    bool jumping = false;

    [SerializeField]
    [Range(0, 1)]
    float landEndLagTime = 0.5f;
    float landCountdown = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (!blackboard)
            blackboard = GameObject.Find("GameManager").GetComponent<Blackboard>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blackboard.togglePause();
        }
        if(blackboard.IsPaused)
            return;
        
        Vector3 dir = getMovedir();
        if (dir != Vector3.zero && landCountdown <= 0)
        {
            transform.rotation = Quaternion.LookRotation(dir);
            moveVelocity += moveAcceleration * Time.deltaTime;
            moveVelocity = Mathf.Clamp(moveVelocity, 0, maxSpeed);
            idleCountdown = idleTime;
        }
        else
        {
            if (moveVelocity > 0)
                moveVelocity -= moveDeceleration * Time.deltaTime;
            else
                moveVelocity = 0;
            dir = Vector3.zero;
        }
        Vector3 moveDir = transform.forward;
        if (moveDir != Vector3.zero)
        {
            if(!jumping)
                checkStep(moveDir * moveVelocity * Time.deltaTime);
            while (checkWall(moveDir * moveVelocity * Time.deltaTime, out Vector3 colNorm))
                moveDir = Vector3.ProjectOnPlane(moveDir, colNorm);
        }
        transform.position += moveDir * moveVelocity * Time.deltaTime;
        animator.SetFloat("MoveSpeed", moveVelocity);


        if (landCountdown > 0)
            landCountdown -= Time.deltaTime;

        if (!jumping && punchAnimationCountdown <= 0 && Input.GetKeyDown(KeyCode.Space))
            startJump();
        if (jumping)
            resolveJumping();

        if (!jumping && !checkGrounded())
        {
            jumping = true;
            animator.SetBool("Jumping", true);
        }


        if (Input.GetMouseButtonDown(0) && !jumping)
            attack();

        if (punchAnimationCountdown > 0)
            punchAnimationCountdown -= Time.deltaTime;

        animator.SetFloat("IdleCountdown", idleCountdown);
        if (idleCountdown > 0 && moveVelocity <= 0)
            idleCountdown -= Time.deltaTime;
    }

    void startJump()
    {
        jumpVelocity = innitialJumpVelocity;
        jumping = true;
        animator.SetBool("Jumping", true);
    }

    void resolveJumping()
    {
        transform.position += Vector3.up * jumpVelocity * Time.deltaTime;
        jumpVelocity -= jumpDeceleration * Time.deltaTime;
        int layerMask = 1 << LayerMask.NameToLayer("Environment");
        if (checkGrounded(2, out RaycastHit hit) && jumpVelocity <= 0)
        {
            if (hit.distance < 1)
            {
                transform.position = hit.point;
                jumpVelocity = 0;
                jumping = false;
            }
            if (hit.distance < 1.25f)
                animator.SetBool("Jumping", false);
        }
    }

    Vector3 getMovedir()
    {
        if (punchAnimationCountdown > 0)
            return Vector3.zero;
        Vector3 dir = Vector3.zero;

        Vector3 camforward = Camera.main.transform.forward;
        camforward.y = 0;
        Vector3 camright = Camera.main.transform.right;
        camright.y = 0;
        if (Input.GetKey(KeyCode.W)) dir += camforward.normalized;
        if (Input.GetKey(KeyCode.S)) dir -= camforward.normalized;
        if (Input.GetKey(KeyCode.D)) dir += camright.normalized;
        if (Input.GetKey(KeyCode.A)) dir -= camright.normalized;

        return dir.normalized;
    }

    void attack()
    {
        if (punchAnimationCountdown <= 0)
        {
            animator.SetTrigger("Punch");
            punchAnimationCountdown = punchAnimationTime;
            idleCountdown = idleTime;
        }
    }

    void checkStep(Vector3 velocity)
    {
        Vector3 origin = transform.position + (transform.up * height) + velocity + transform.forward * playerRadius;
        Vector3 perp = new Vector3(-velocity.z, 0, velocity.x);
        int layermask = 1 << LayerMask.NameToLayer("Environment");
        int iterateDir = 1;

        for (int i = 0; i < 3; i++)
        {
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, height * 2, layermask))
            {
                if (hit.distance < height * 1.1f && hit.distance >= height * 0.75f)
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                }
            }
            origin += perp * iterateDir;
            iterateDir *= -2;
        }
    }

    bool checkWall(Vector3 velocity, out Vector3 normal)
    {
        normal = Vector3.zero;
        Vector3 dir = velocity.normalized;
        Vector3 dirPerp = new Vector3(dir.z, dir.y, -dir.x);
        Vector3 origin = transform.position + Vector3.up * height /** 0.9f*/ + dirPerp * playerRadius - dir * playerRadius;
        float range = velocity.magnitude + playerRadius * 2;

        RaycastHit closest = new RaycastHit();

        bool objectHit = false;

        int layermask = 1 << LayerMask.NameToLayer("Environment");

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Debug.DrawLine(origin, origin + dir * range);
                if (Physics.Raycast(origin, dir, out RaycastHit hit, range, layermask))
                {
                    if (!objectHit || hit.distance < closest.distance)
                        closest = hit;
                    objectHit = true;
                }
                origin += -dirPerp * playerRadius;
            }
            origin += dirPerp * playerRadius * 3 - (transform.up * height * 0.45f);
        }

        if (objectHit)
        {
            normal = closest.normal;
            normal.y = 0;
            normal.Normalize();
            return true;
        }
        else
            return false;
    }

    bool checkGrounded()
    {
        bool grounded = false;
        Vector3 origin = transform.position + Vector3.up * height * 0.5f + transform.forward * playerRadius + transform.right * playerRadius;

        int layermask = 1 << LayerMask.NameToLayer("Environment");

        int dirInverted = -1;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (Physics.Raycast(origin, Vector3.down, height * 0.5f, layermask))
                    grounded = true;
                origin += transform.right * playerRadius * dirInverted;
            }
            dirInverted *= -1;
            origin += -transform.forward * playerRadius;
        }

        return grounded;
    }
    bool checkGrounded(float distance, out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * height * 0.5f + transform.forward * playerRadius + transform.right * playerRadius;

        int layermask = 1 << LayerMask.NameToLayer("Environment");

        int dirInverted = -1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, distance, layermask))
                {
                    hit = hitInfo;
                    return true;
                }
                origin += transform.right * playerRadius * dirInverted;
            }
            dirInverted *= -1;
            origin += -transform.forward * playerRadius;
        }
        hit = new RaycastHit();
        return false;
    }
}
