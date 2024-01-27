using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    public Boss boss;
    public int armDamage;

    [Header("non toccare")]
    public Collider coll;
    public bool isAttacking;
    private void OnEnable()
    {
        coll = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (player.isDashing && !isAttacking)
            {
                boss.TakeDamage(player.damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (isAttacking)
            {
                player.takeDamage(armDamage);
            }
        }
    }
    
}
