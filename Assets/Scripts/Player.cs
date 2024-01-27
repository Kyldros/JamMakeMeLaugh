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
    [SerializeField] private float ragdollImmuneDuration;
    [SerializeField] private float ragdollImmuneDurationUpgrade;
    [SerializeField] private float ragdollCooldown;

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
    [SerializeField] private float tPoseCooldown;
    [SerializeField] private int healOnTPose;
    [SerializeField] private int healOnTPoseUpgrade;

    [Header("Damage System")]
    public int maxHp = 5;
    public int damage = 1;
    public int damageIncreaser = 1;
    public float immunityDuration;

    [Header("Audio")]
    public AudioManager audioManager;
    public AudioClip[] stepsClips;
    public AudioClip[] clipsRagdoll;
    public AudioClip clipDash;
    public AudioClip tPoseClip;

    [Header("Material")]
    [SerializeField] private Material mat;
    [SerializeField] private Color dashColor = Color.blue;
    [SerializeField] private Color ragdollColor = Color.red;
    [SerializeField] private Color TPoseColor = Color.yellow;




    [Header("Non toccare chiedi al programmer"), Description("si capito bene, non toccare o ti taglio il bisnelo")]
    public bool canTakeDamage = true;
    public GameObject botMesh;
    public GameObject botParent;
    public GameObject boneToMove;
    public Rigidbody rb;
    public CapsuleCollider coll;
    public int currentHP = 5;
    public bool ragdollUnlocked;
    public bool dashUnlocked;
    public bool TPoseUnlocked;
    public bool isDashing = false;
    public bool canTPose;
    public bool canRagdoll;
    public bool isShooted;
    private bool canDash = true;
    public bool isGrounded;

    private bool tPoseHeals;


    private List<Rigidbody> ragdollRb;
    private List<Collider> ragdollColl;
    private Vector2 moveDirection;
    private Vector3 lastMovement;
    public bool isRagdoll { get; private set; } = false;
    private bool isTPose = false;
    private bool isMoving;
    private bool canGetUp;
    private bool isJumping;

    private GameObject TposeAudioClipObject;

    private Vector3 tPoseStartAngle;
    Vector3 movement;



    private Animator anim;
    private bool isSprinting;
    private bool tempBool;


    public UnityEvent onDamage = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    private PlatformerEffector3D currentPlatform;

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
        
        if (isJumping && Physics.Raycast(transform.position, Vector3.down, groundCheckDistance/2, groundLayer))
        {
            isGrounded = true;
            anim.SetTrigger("isGrounded");
            isJumping = false;


        }
        else if (!isDashing && !isTPose)
        {
            if (!isShooted)
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
        if (transform.position.z != -10)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
    public void OnEnable()
    {
        ragdollRb = GetComponentsInChildren<Rigidbody>().ToList();
        ragdollRb.RemoveAt(0);

        ragdollColl = GetComponentsInChildren<Collider>().ToList();
        ragdollColl.RemoveAt(0);


        anim = GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
    }

    //Guardare dopo riposino
    public LayerMask GetGroundLayer()
    {
        return groundLayer;
    }
    public void Ragdoll(InputAction.CallbackContext context)
    {
        if (context.performed && ragdollUnlocked && !isDashing && !isTPose && canRagdoll)
        {
            SetRagdoll(!isRagdoll);
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && dashUnlocked && !isTPose && !isRagdoll)
        {
            if (!isDashing && canDash)
            {
                canDash = false;
                isDashing = true;
                GameManager.Instance.TriggerWalls(!isDashing);
                anim.SetTrigger("isDashing");
                SetOutlineColor(dashColor);
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
            if (!isRagdoll)
            {
                anim.SetTrigger("isJumping");
            }
            isJumping = true;
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
    public void TPose(InputAction.CallbackContext context)
    {
        if (context.performed && dashUnlocked && canTPose && !isDashing && !isRagdoll)
        {
            if (!isDashing && !isTPose)
            {
                canDash = false;
                canTPose = false;
                isTPose = true;
                anim.SetTrigger("isTPose");
                SetOutlineColor(TPoseColor);

                if (tPoseHeals)
                {
                    HealPlayer(tPoseHeals);
                }

                StartCoroutine(nameof(startTPoseCooldown));
                StartCoroutine(nameof(timerEndTPose));
            }
        }
    }
    private void HealPlayer(bool tPoseHeals)
    {
        throw new System.NotImplementedException();
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = new Vector2(context.ReadValue<Vector2>().x, 0);
        if (currentPlatform != null && context.ReadValue<Vector2>().y < 0)
            currentPlatform.DisableCollider();
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

        movement = new Vector3(moveDirection.x, 0, moveDirection.y);

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
            tPoseStartAngle = botParent.transform.rotation.eulerAngles;
            botParent.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        isMoving = rb.velocity.magnitude > 0.2f;
        anim.SetBool("isMoving", isMoving);

    }
    public void SetRagdoll(bool value)
    {
        anim.enabled = !value;
        isRagdoll = value;


        rb.velocity = Vector3.zero;



        //rb.velocity = Vector3.zero;


        foreach (Rigidbody rb in ragdollRb)
        {
            //?
            rb.isKinematic = !value;
        }
        foreach (Collider col in ragdollColl)
        {
            col.enabled = value;
        }

        if (value == true)
        {
            PlayRagdollClip(clipsRagdoll);
            SetRagdollImmune();

            SetOutlineColor(ragdollColor);
            coll.height = 0.3f;
            coll.center = new Vector3(0, 0.325f, 0);

        }
        else
        {
            coll.height = 1.7f;
            coll.center = new Vector3(0, 0.75f, 0);
            canRagdoll = false;
            StartCoroutine(nameof(startRagdollCooldown));
            TurnOffOutline();
        }

    }
    public void takeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHP -= damage;
            Debug.Log(currentHP);
            if (currentHP <= 0)
                onDeath?.Invoke();
            else
                onDamage?.Invoke();

            SetNormalImmune();
        }

    }
    public IEnumerator startDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Debug.Log("Dash Ready");
    }
    public IEnumerator startTPoseCooldown()
    {


        yield return new WaitForSeconds(tPoseCooldown);
        canTPose = true;
        Debug.Log("Tpose Ready");

    }
    public IEnumerator startRagdollCooldown()
    {
        yield return new WaitForSeconds(ragdollCooldown);
        canRagdoll = true;
        Debug.Log("ragdoll Ready");
    }
    public IEnumerator timerEndDash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        GameManager.Instance.TriggerWalls(!isDashing);
        rb.velocity = Vector3.zero;
        TurnOffOutline();
        anim.SetTrigger("isExitingDashing");


    }
    public IEnumerator timerEndTPose()
    {
        TposeAudioClipObject = Instantiate(new GameObject());
        AudioSource source = TposeAudioClipObject.AddComponent<AudioSource>();
        source.clip = tPoseClip;
        source.Play();

        yield return new WaitForSeconds(tPoseDuration);

        isTPose = false;
        canDash = true;
        rb.velocity = Vector3.zero;
        TurnOffOutline();

        TposeAudioClipObject.GetComponent<AudioSource>().mute = true;
        TposeAudioClipObject.GetComponent<AudioSource>().Stop();
        TposeAudioClipObject.SetActive(false);
        Destroy(TposeAudioClipObject.gameObject);

        anim.SetTrigger("isExitingTPose");

        botParent.transform.rotation = Quaternion.Euler(tPoseStartAngle);

    }
    public void PlayStepClip()
    {
        int randomInt = UnityEngine.Random.Range(0, stepsClips.Length);
        AudioClip clipToPlay = stepsClips[randomInt];
        audioManager.PlayAudio(clipToPlay);
    }
    public void PlayRagdollClip(AudioClip[] clipList)
    {
        int randomInt = UnityEngine.Random.Range(0, clipList.Length);
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
    private void SetNormalImmune()
    {
        canTakeDamage = false;
        mat.SetFloat("_timeMulty", 1);
        StartCoroutine(nameof(ResetCanTakeDamage));
    }
    private void SetRagdollImmune()
    {
        canTakeDamage = false;
        StartCoroutine(nameof(ResetCanTakeDamage));
    }
    public IEnumerator ResetCanTakeDamage()
    {
        yield return new WaitForSeconds(immunityDuration);
        mat.SetFloat("_timeMulty", 0);
        canTakeDamage = true;
    }
    public void SetOutlineColor(Color colorToGive)
    {
        mat.SetColor("_Color", colorToGive);

    }
    public void TurnOffOutline()
    {
        mat.SetColor("_Color", new Color(0, 0, 0, 0));
    }
    public void UpgradeRandomAbility()
    {
        int tempInt = Random.Range(0, 3);
        switch (tempInt)
        {
            case 0:
                IncreaseDamage();
                break;

            case 1:
                IncreaseRagdollImmunity();
                break;

            case 2:
                IncreaseTPoseHeal();
                break;

        }

    }
    public void IncreaseDamage()
    {
        damage += damageIncreaser;
    }
    public void IncreaseRagdollImmunity()
    {
        ragdollImmuneDuration += ragdollImmuneDurationUpgrade;
    }
    public void IncreaseTPoseHeal()
    {
        healOnTPose += healOnTPoseUpgrade;
    }
    public void StopDash()
    {

    }
    internal void UnregisterPlatform(PlatformerEffector3D platformerEffector3D)
    {
        if (currentPlatform != null && currentPlatform == platformerEffector3D)
        {
            currentPlatform = null;
        }
    }
    internal void RegisterPlatform(PlatformerEffector3D platformerEffector3D)
    {
        currentPlatform = platformerEffector3D;
    }

    public void WinGame(GameObject dancePoint)
    {
        GameManager.Instance.DisablePlayerInput();
        transform.position = dancePoint.transform.position;
        anim.applyRootMotion = true;
        rb.velocity = Vector3.zero;
        anim.SetTrigger("BossDead");
        GameManager.Instance.audioManager.PlayMusic(GameManager.Instance.bossFightMusic);
    }

}

