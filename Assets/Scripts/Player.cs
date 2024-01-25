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
    [SerializeField] private float sprintSpeed;
    public GameObject mesh;
    public GameObject boneToMove;
    public Rigidbody rb;
    public Collider coll;
    private List<Rigidbody> ragdollRb;
    private List<Collider> ragdollColl;

    private Vector2 moveDirection;
    private Vector3 lastMovement;
    private bool isRagdoll = false;
    private bool isMoving;
    private Animator anim;
    private bool isSprinting;

    public int maxHp = 5;
    public int currentHP = 5;
    public int damage = 1;

    public UnityEvent onDamage = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();



    private void Start()
    {
        currentHP = maxHp;
        foreach (Rigidbody rb in ragdollRb)
        {
            rb.isKinematic = true;
        }
        foreach (Collider col in ragdollColl)
        {
            col.enabled = false;
        }


    }
    private void Update()
    {
        Move2(moveDirection);
        if (isRagdoll)
        {
            boneToMove.GetComponent<Rigidbody>().MovePosition(transform.position);
        }

    }

    public void OnEnable()
    {
        ragdollRb = GetComponentsInChildren<Rigidbody>().ToList();
        ragdollRb.RemoveAt(0);

        ragdollColl = GetComponentsInChildren<Collider>().ToList();
        ragdollColl.RemoveAt(0);

        anim = GetComponentInChildren<Animator>();
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
        moveDirection = context.ReadValue<Vector2>();

    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isSprinting = true;
            anim.SetBool("isSprinting", isSprinting);
        }
        if(context.canceled)
        {
            isSprinting = false;
            anim.SetBool("isSprinting", isSprinting);
        }
    }
    public void Move2(Vector2 moveDirection)
    {

        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);


        if (isRagdoll && !isSprinting)
        {
            rb.velocity = new Vector3(moveDirection.x * ragdollSpeed, rb.velocity.y, moveDirection.y * ragdollSpeed);
        }

        else
        {
            rb.velocity = new Vector3(moveDirection.x * walkSpeed, rb.velocity.y, moveDirection.y * walkSpeed);

                if (isSprinting)
                rb.velocity = new Vector3(moveDirection.x * sprintSpeed, rb.velocity.y, moveDirection.y * sprintSpeed);

        }


        if (lastMovement != movement && !isRagdoll)
        {
            lastMovement = movement;

            if (movement == Vector3.right)
            {
                mesh.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (movement == Vector3.left)
            {
                mesh.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }
        }

        isMoving = rb.velocity.magnitude > 0.2f;
        anim.SetBool("isMoving", isMoving);


    }
    public void SetRagdoll(bool value)
    {
        anim.enabled = !value;
        isRagdoll = value;
        rb.velocity = Vector3.zero;


        foreach (Rigidbody rb in ragdollRb)
        {
            rb.isKinematic = !value;
        }
        foreach (Collider col in ragdollColl)
        {
            col.enabled = value;
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

