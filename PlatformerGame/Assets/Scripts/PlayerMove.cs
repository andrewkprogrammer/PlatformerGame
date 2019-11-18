using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMove : MonoBehaviour
{
    Animator animator;

    [Header("Components")]
    [SerializeField] SwordHit sword;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("How fast the player moves")]
    [Range(0, 50)]
    float moveSpeed = 1.0f;

    [Header("Attack")]
    [SerializeField]
    int damage = 1;
    [SerializeField]
    int comboAttacks = 2;
    int attacknum = 0;

    bool wasAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sword.damage = damage;
        sword.player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkBlock())
        {
            Vector3 dir = getMovedir();
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

            }
            animator.SetFloat("Speed", (dir * moveSpeed).magnitude);

            attack();
        }
    }

    bool checkBlock()
    {
        if (Input.GetMouseButton(2))
        {
            animator.SetBool("Blocking", true);
            return true;
        }
        else
        {
            animator.SetBool("Blocking", false);
            return false;
        }
    }

    void attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && wasAttacking)
        {
            sword.attacking = false;
        }
        if (Input.GetMouseButtonUp(0) && !wasAttacking)
        {
            animator.SetTrigger("Attack");
            sword.attacking = true;
        }
        wasAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("attack2");
    }

    Vector3 getMovedir()
    {
        if (!wasAttacking)
        {
            Vector3 dir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) dir.z += 1;
            if (Input.GetKey(KeyCode.S)) dir.z -= 1;
            if (Input.GetKey(KeyCode.D)) dir.x += 1;
            if (Input.GetKey(KeyCode.A)) dir.x -= 1;

            return dir.normalized;
        }
        else return Vector3.zero;
    }
}
