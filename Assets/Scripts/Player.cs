using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float ragdollSpeed;
    [SerializeField] private float sprintSpeed;

    [Header("Ragdoll")]
    [SerializeField] private float ragdollTimer;

    [Header("Dash")]
    [SerializeField] private float dashMulty;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    [Header("Damage System")]
    public int maxHp = 5;
    public int damage = 1;

    [Header("Audio")]
    public AudioManager audioManager;
    public AudioClip[] stepsClips;
    public AudioClip clipRagdoll;
    public AudioClip clipDash;


    
    [Header("Non toccare chiedi al programmer"), Description("si capito bene, non toccare o ti taglio il bisnelo")]
    public GameObject botMesh;
    public GameObject botParent;
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
    private bool canGetUp;
    private bool isDashing = false;
    private bool canDash = true;
    private Animator anim;
    private bool isSprinting;
    private bool tempBool;


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
            //dash
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
            
            SetRagdoll(!isRagdoll,false);
        }

    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isDashing && canDash)
            {
                canDash = false;
                isDashing = true;
                anim.SetBool("isDashing", isDashing);
                Debug.Log("isDashing " + isDashing);
                audioManager.PlayAudio(clipDash);
                
                StartCoroutine(nameof(startDashCooldown));
                StartCoroutine(nameof(timerEndDash));
            }
        }
    }  
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = new Vector2(context.ReadValue<Vector2>().x, 0);

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
        botMesh.transform.localPosition = Vector3.zero;
        botMesh.transform.localRotation = new Quaternion(0,0,0,0);

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
                botParent.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (movement == Vector3.left)
            {
                botParent.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }
        }

        isMoving = rb.velocity.magnitude > 0.2f;
        anim.SetBool("isMoving", isMoving);

    }
    public void SetRagdoll(bool value,bool hasTimer)
    {
        if (!hasTimer && !tempBool) 
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
            audioManager.PlayAudio(clipRagdoll);

           
        }

        if (hasTimer || tempBool)
        {
            if (canGetUp)
            {
                anim.enabled = true;
                isRagdoll = false;
                rb.velocity = Vector3.zero;

                foreach (Rigidbody rb in ragdollRb)
                {
                    rb.isKinematic = true;
                }
                foreach (Collider col in ragdollColl)
                {
                    col.enabled = false;
                }
            }
            else
            {

                anim.enabled = false;
                isRagdoll = true;
                rb.velocity = Vector3.zero;

                foreach (Rigidbody rb in ragdollRb)
                {
                    rb.isKinematic = false;
                }
                foreach (Collider col in ragdollColl)
                {
                    col.enabled = true;
                }
                audioManager.PlayAudio(clipRagdoll);

                canGetUp = false;               
                StartCoroutine(nameof(timerRagdool));
            }
            
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
        Debug.Log("Dash Ready");
    }
    public IEnumerator timerEndDash()
    {
        yield return new WaitForSeconds(dashDuration); ;
        isDashing = false;
        rb.velocity = Vector3.zero;
        anim.SetBool("isDashing", isDashing);
        Debug.Log("isDashing " + isDashing);
    }
    public IEnumerator timerRagdool()
    {
       
        yield return new WaitForSeconds(ragdollTimer);
        canGetUp = true;
        tempBool = true;
    }

    public void PlayStepClip(AudioClip[] clipList)
    {
        int randomInt = UnityEngine.Random.Range(0, clipList.Length-1);
        AudioClip clipToPlay = clipList[randomInt];
        audioManager.PlayAudio(clipToPlay);


    }
}

