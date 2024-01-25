using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    public int maxHp = 5;
    public int currentHP = 5;
    public int damage = 1;

    public UnityEvent onDamage = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    private bool isRagdoll;

    private void Start()
    {
        currentHP = maxHp;
    }

    public    void OnEnable()
    {
       
    }

    public void SetRagdoll()
    {
        
    }

    public void Dash()
    {
        
    }

    public void Move()
    {
        
    }

    internal void takeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
            onDeath?.Invoke();
        else
            onDamage?.Invoke();
    }
}
