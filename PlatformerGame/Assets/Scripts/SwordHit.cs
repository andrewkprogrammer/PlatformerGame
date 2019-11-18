using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public bool attacking = false;
    public int damage;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (attacking && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyScript>().GetHit(damage, player.transform.forward);
        }
    }

    
}
