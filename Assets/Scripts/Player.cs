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

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("TPose")]
    [SerializeField] private float tPoseSpeed;
    [SerializeField] private float tPoseDuration;

    [Header("Damage System")]
    public int maxHp = 5;
    public int damage = 1;
    public int damageIncreaser = 1;
    public float immunityDuration;

    [Header("Audio")]
    public AudioManager audioManager;
    public AudioClip[] stepsClips;
    public AudioClip clipRagdoll;
    public AudioClip clipDash;
    public AudioClip tPoseClip;

    [Header("Material")]
    [SerializeField] private Material mat;


    [Header("Non toccare chiedi al programmer"), Description("si capito bene, non toccare o ti taglio il bisnelo")]
    public GameObject botMesh;
    public GameObject botParent;
    public GameObject boneToMove;
    public Rigidbody rb;
    public Collider coll;
    public int currentHP = 5;
    public bool ragdollUnlocked;
    public bool dashUnlocked;
    public bool TPoseUnlocked;
    public bool isDashing = false;

    private bool canTakeDamage;
    private bool isGrounded;
    private List<Rigidbody> ragdollRb;
    private List<Collider> ragdollColl;
    private Vector2 moveDirection;
    private Vector3 lastMovement;
    private bool isRagdoll = false;
    private bool isTPose = false;
    private bool isMoving;
    private bool canGetUp;
   
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
        if (!isDashing && !isTPose)
        {
            Move2(moveDirection);
            if (isRagdoll)
            {
                boneToMove.GetComponent<Rigidbody>().MovePosition(transform.position);
            }
        }
        else if (isDashing)
        {
            rb.velocity = Vector3.zero;
            //dash
            Move2(lastMovement);

        }
        else if (isTPose)
        {
            rb.velocity = Vector3.zero;
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
        if (context.performed && ragdollUnlocked)
        {
            SetRagdoll(!isRagdoll, false);
        }

    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && dashUnlocked)
        {
            if (!isDashing && canDash)
            {
                canDash = false;
                isDashing = true;
                GameManager.Instance.TriggerWalls(!isDashing);
                anim.SetTrigger("isDashing");
                Debug.Log("isDashing " + isDashing);
                audioManager.PlayAudio(clipDash);

                StartCoroutine(nameof(startDashCooldown));
                StartCoroutine(nameof(timerEndDash));
            }
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
        {
            // Add force in the upward direction to simulate jumping
            Debug.Log("Isjumping");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
    public void TPose(InputAction.CallbackContext context)
    {
        if (context.performed && dashUnlocked)
        {
            if (!isDashing && !isTPose)
            {
                canDash = false;
                isTPose = true;
                anim.SetTrigger("isTPose");
                Debug.Log("isTPose " + isTPose);
                audioManager.PlayAudio(tPoseClip);


                StartCoroutine(nameof(timerEndTPose));
            }
        }
    }
    private void SetTPose(bool value)
    {
        canDash = !value;
        isTPose = value;
        rb.useGravity = !value;
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
        botMesh.transform.localRotation = new Quaternion(0, 0, 0, 0);

        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);

        if (isRagdoll && !isSprinting && !isDashing && !isTPose)
        {
            rb.velocity = new Vector3(moveDirection.x * ragdollSpeed, rb.velocity.y, moveDirection.y * ragdollSpeed);
        }

        else if (!isRagdoll && !isSprinting && !isTPose)
        {
            if (isDashing)
            {
                rb.velocity = new Vector3(moveDirection.x * walkSpeed * dashMulty, rb.velocity.y, moveDirection.y * walkSpeed * dashMulty);
            }
            else
            {
                rb.velocity = new Vector3(moveDirection.x * walkSpeed, rb.velocity.y, moveDirection.y * walkSpeed);
            }

        }
        else if (isSprinting && !isTPose)
        {
            if (isDashing)
            {
                rb.velocity = new Vector3(moveDirection.x * sprintSpeed * dashMulty, rb.velocity.y, moveDirection.y * sprintSpeed * dashMulty);
            }
            else
            {
                rb.velocity = new Vector3(moveDirection.x * sprintSpeed, rb.velocity.y, moveDirection.y * sprintSpeed);
            }

        }
        else if (isTPose)
        {
            rb.velocity = new Vector3(moveDirection.x * tPoseSpeed, 0, moveDirection.y * tPoseSpeed);
        }
        if (lastMovement != movement && !isRagdoll && !isTPose)
        {
            if (movement != Vector3.zero)
            {
                lastMovement = movement;
            }
            if (movement == Vector3.right)
            {
                botParent.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (movement == Vector3.left)
            {
                botParent.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }
        }
        else if (isTPose)
        {
            botParent.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        isMoving = rb.velocity.magnitude > 0.2f;
        anim.SetBool("isMoving", isMoving);

    }
    public void SetRagdoll(bool value, bool hasTimer)
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
        if (canTakeDamage)
        {
            currentHP -= damage;
            if (currentHP <= 0)
                onDeath?.Invoke();
            else
                onDamage?.Invoke();

            SetImmune();
        }

    }
    public IEnumerator startDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Debug.Log("Dash Ready");
    }
    public IEnumerator timerEndDash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        GameManager.Instance.TriggerWalls(!isDashing);
        rb.velocity = Vector3.zero;
        anim.SetTrigger("isExitingDashing");
        Debug.Log("isDashing " + isDashing);
    }
    public IEnumerator timerEndTPose()
    {
        yield return new WaitForSeconds(tPoseDuration);
        isTPose = false;
        canDash = true;
        rb.velocity = Vector3.zero;
        anim.SetTrigger("isExitingTPose");
        Debug.Log("isTPose " + isTPose);
    }
    public IEnumerator timerRagdool()
    {

        yield return new WaitForSeconds(ragdollTimer);
        canGetUp = true;
        tempBool = true;
    }
    public void PlayStepClip(AudioClip[] clipList)
    {
        int randomInt = UnityEngine.Random.Range(0, clipList.Length - 1);
        AudioClip clipToPlay = clipList[randomInt];
        audioManager.PlayAudio(clipToPlay);


    }
    public void UnlockRagdoll()
    {
        ragdollUnlocked = true;
    }
    public void UnlockDash()
    {
        dashUnlocked = true;
    }
    public void UnlockTPose()
    {
        TPoseUnlocked = true;
    }
    public void IncreaseDamage()
    {
        damage += damageIncreaser;
    }
   private void SetImmune()
    {
        canTakeDamage = false;
        
        StartCoroutine(nameof(immunityDuration));
    }
    public IEnumerator ResetCanTakeDamage()
    {
        yield return new WaitForSeconds(immunityDuration);
        canTakeDamage=true;
    }


}

