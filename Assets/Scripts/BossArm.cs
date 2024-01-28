using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    public Boss boss;
    public int armDamage;
    public float armMoveSpeed;
    public float armDropSpeed;

    [Header("non toccare")]
    public Collider coll;
    public Rigidbody rb;
    public bool isAttacking;
    public GameObject dropReachHeight;
    public GameObject startPoint;
    public GameObject pivot;
    public bool isRight;
    public AudioClip clipTestata;
    public bool canTakeDamage = true;
    private void OnEnable()
    {
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            
            if (player.isDashing && !isAttacking && boss.alive && canTakeDamage)
            {
                Debug.Log(boss.currentHp);
                GameManager.Instance.audioManager.PlayAudio(clipTestata);
                canTakeDamage = false;
                boss.TakeDamage(player.damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {         
            if (isAttacking && boss.alive)
            {
                player.takeDamage(armDamage);
            }
        }
    }
    
}
