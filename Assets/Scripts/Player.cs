using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{


    [SerializeField] private float walkSpeed;
    [SerializeField] private float ragdollSpeed;
    public Rigidbody rb;
    public List<Rigidbody> ragdollRb;

    private bool isRagdoll = false;
    private Animator anim;

    public int maxHp = 5;
    public int currentHP = 5;
    public int damage = 1;

    public UnityEvent onDamage = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();



    private void Start()
    {
        currentHP = maxHp;
    }

    public void OnEnable()

    {
        ragdollRb = GetComponentsInChildren<Rigidbody>().ToList();
        ragdollRb.RemoveAt(0);
        anim = GetComponent<Animator>();
    }

    public void Ragdoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetRagdoll(!isRagdoll);
        }

    }

    public void Dash()
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector3 movementInput = context.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y);

        if (!isRagdoll)
            rb.velocity = movement * walkSpeed;
        else
            rb.velocity = movement * ragdollSpeed;




        //isMoving = rb.velocity.magnitude > 0.2f;

    }


    public void SetRagdoll(bool value)
    {
        anim.enabled = !value;
        isRagdoll = value;
        rb.isKinematic = value;
        foreach (Rigidbody rb in ragdollRb)
        {
            rb.isKinematic = !value;
        }
    }
        public void takeDamage(int damage)
        {
            currentHP -= damage;
            if (currentHP <= 0)
                onDeath?.Invoke();
            else
                onDamage?.Invoke();

        }
    }

