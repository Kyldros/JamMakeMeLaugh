using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Variable")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float ragdollSpeed;
    [SerializeField] private float sprintSpeed;

    [Header("Dash")]
    [SerializeField] private float dashMulty;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    [Header("Damage System")]
    public int maxHp = 5;
    public int damage = 1;

    [Header("Non toccare chiedi al programmer"), Description("si capito bene, non toccare o ti taglio il bisnelo")]
    public GameObject mesh;
    public GameObject boneToMove;
    public Rigidbody rb;
    public Collider coll;
    public int currentHP = 5;


    private float WalkSpeed => isDashing ? walkSpeed * dashMulty : walkSpeed;
    private float RagdollSpeed => isDashing ? ragdollSpeed * dashMulty : ragdollSpeed;
    private List<Rigidbody> ragdollRb;
    private List<Collider> ragdollColl;
    private Vector2 moveDirection;
    private Vector3 lastMovement;
    private bool isRagdoll = false;
    private bool isMoving;
    private bool isDashing = false;
    private bool canDash;
    private Animator anim;
    private bool isSprinting;


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
        if (!isDashing)
        {
            Move2(moveDirection);
            if (isRagdoll)
            {
                boneToMove.GetComponent<Rigidbody>().MovePosition(transform.position);
            }
        }
        else
        {
            Move2(lastMovement);
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
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isDashing && !isRagdoll && canDash)
                canDash = false;
                isDashing = true;
                anim.SetBool("isDashing", isDashing);
               Debug.Log("isDashing " + isDashing);
            

            StartCoroutine(nameof(startDashCooldown));
            StartCoroutine(nameof(timerEndDash));
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();

    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed && rb.velocity != Vector3.zero)
        {
            isSprinting = true;
            anim.SetBool("isSprinting", isSprinting);
        }
        if (context.canceled)
        {
            isSprinting = false;
            anim.SetBool("isSprinting", isSprinting);
        }
    }
    public void Move2(Vector2 moveDirection)
    {

        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);


        if (isRagdoll && !isSprinting && !isDashing)
        {
            rb.velocity = new Vector3(moveDirection.x * RagdollSpeed, rb.velocity.y, moveDirection.y * RagdollSpeed);
        }

        else
        {
            rb.velocity = new Vector3(moveDirection.x * WalkSpeed, rb.velocity.y, moveDirection.y * WalkSpeed);

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

    public IEnumerator startDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public IEnumerator timerEndDash()
    {
        yield return new WaitForSeconds(dashDuration); ;
        isDashing = false;
        rb.velocity = Vector3.zero;
        anim.SetBool("isDashing", isDashing);
        Debug.Log("isDashing " + isDashing);
    }
}

