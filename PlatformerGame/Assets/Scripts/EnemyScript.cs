using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    int maxHealth = 3;
    int health;

    [Header("Knockback")]
    [SerializeField]
    float knockbackDistance = 1.0f;
    [SerializeField]
    float knockbackSpeed = 5.0f;
    float knockbackCountdown = 0;
    float getKnockbackTime { get { return knockbackDistance / knockbackSpeed; } }

    Renderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(knockbackCountdown > 0)
        {
            knockbackCountdown -= Time.deltaTime;
            transform.position -= transform.forward * knockbackSpeed * Time.deltaTime;
            if (knockbackCountdown <= 0)
                renderer.material.color = Color.white;
        }

        if (health == 0)
            Destroy(gameObject);
    }

    public void GetHit(int damage, Vector3 direction)
    {
        if (knockbackCountdown <= 0)
        {
            transform.rotation = Quaternion.LookRotation(-direction);
            knockbackCountdown = getKnockbackTime;
            health -= damage;
            renderer.material.color = Color.red;
        }
    }
}
