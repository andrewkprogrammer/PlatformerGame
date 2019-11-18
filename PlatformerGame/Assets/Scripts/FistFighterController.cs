using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FistFighterController : MonoBehaviour
{
    Animator animator;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("How fast the player moves")]
    [Range(0, 50)]
    float moveAcceleration = 5.0f;
    [SerializeField]
    float maxSpeed = 10.0f;
    float moveVelocity = 0.0f;

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


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = getMovedir();
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
            moveVelocity += moveAcceleration * Time.deltaTime;
            moveVelocity = Mathf.Clamp(moveVelocity, 0, maxSpeed);
            transform.position += transform.forward * moveVelocity * Time.deltaTime;
            idleCountdown = idleTime;
        }
        else
        {
            moveVelocity = 0;
        }
        animator.SetFloat("MoveSpeed", (dir * moveVelocity).magnitude);



        if (punchAnimationCountdown <= 0 && Input.GetKeyDown(KeyCode.Space))
            startJump();
        if(jumping)
        {
            transform.position += Vector3.up * jumpVelocity * Time.deltaTime;
            jumpVelocity -= jumpDeceleration * Time.deltaTime;
            int layerMask = 1 << LayerMask.NameToLayer("Floor");
            if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 1, layerMask))
            {
                transform.position = hit.point;
                jumpVelocity = 0;
                jumping = false;
                animator.SetBool("Jumping", false);
            }
        }

        if (Input.GetMouseButtonDown(0))
            attack();

        if (punchAnimationCountdown > 0)
            punchAnimationCountdown -= Time.deltaTime;

        animator.SetFloat("IdleCountdown", idleCountdown);
        if (idleCountdown > 0)
            idleCountdown -= Time.deltaTime;
    }

    void startJump()
    {
        jumpVelocity = innitialJumpVelocity;
        jumping = true;
        animator.SetBool("Jumping", true);
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


}
