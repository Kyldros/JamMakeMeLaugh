using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    public Boss boss;
    public int armDamage;
    public float attackCooldown;
    private bool isAttacking;
    private float timer;
    
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
    private void Update()
    {
        if(timer >= attackCooldown)
        {
            StartAttack();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void StartAttack()
    {
       isAttacking = true;

    }
}
