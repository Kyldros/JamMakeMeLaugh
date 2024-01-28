using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int hpMax;
    [SerializeField] float attackFollowPlayerTimer;
    
    [SerializeField] private BossArm leftArm;
    [SerializeField] private BossArm rightArm;
    [SerializeField] private float attackFollowDuration;
    [SerializeField] private GameObject dancePoint;

    
    public AudioClip[] attacksClips;
    public AudioClip[] normalVoiceClip;
    public AudioClip[] attackVoiceClip;




    private Vector3 playerPos;

    public int currentHp;
    private Animator anim;

    private float timerAttack;
    private float timerFollow;
    private float timer1;
    private float timer2;
    private float timer3;
    private bool raiseArm;
    private bool moveArm;
    private bool currentAttackisRight = false;
    private bool canAttack = true;
   public bool alive;
    
    private void Awake()
    {
        currentHp = hpMax;
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
       
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log(currentHp);
        if (currentHp <= 0 && alive)
        {
            anim.SetTrigger("Death");
            canAttack = false;
            alive = false;
            
            GameManager.Instance.player.WinGame(dancePoint);
            
 
        }
    }


    public void Update()
    {
        if (canAttack)
        {
            if (leftArm.isAttacking)
            {
                if (timerFollow >= attackFollowDuration)
                {
                    if (moveArm)
                    {
                        MoveArm(leftArm);
                    }
                    else if (raiseArm)
                    {
                        RaiseArm(leftArm);
                    }
                    else
                    {
                        DropArm(leftArm);
                    }
                }
                else
                {
                    ArmAttack(leftArm);
                    timerFollow += Time.deltaTime;
                }
            }

            else if (rightArm.isAttacking)
            {
                if (timerFollow >= attackFollowDuration)
                {
                    if (moveArm)
                    {
                        MoveArm(rightArm);
                    }
                    else if (raiseArm)
                    {
                        RaiseArm(rightArm);
                    }
                    else
                    {
                        DropArm(rightArm);
                    }

                }
                else
                {
                    ArmAttack(rightArm);
                    timerFollow += Time.deltaTime;
                }
            }
        }
       
    }

    private void ArmAttack(BossArm arm)
    {
        arm.coll.isTrigger = false;
        timerAttack += arm.armMoveSpeed * Time.deltaTime;
        timerAttack = Mathf.Clamp01(timerAttack);
        playerPos = GameManager.Instance.player.transform.position;
        arm.transform.position = Vector3.Lerp(arm.transform.position, new Vector3(playerPos.x, arm.pivot.transform.position.y, playerPos.z), timerAttack);        
    }
    private void DropArm(BossArm arm)
    {
        arm.coll.isTrigger = true;
        timer1 += arm.armDropSpeed * Time.deltaTime;
        timer1 = Mathf.Clamp01(timer1);
        arm.pivot.transform.position = Vector3.Lerp(arm.pivot.transform.position, new Vector3(arm.pivot.transform.position.x, arm.dropReachHeight.transform.position.y,arm.pivot.transform.position.z), timer1);

        if (Vector3.Distance(arm.pivot.transform.position, new Vector3(arm.pivot.transform.position.x, arm.dropReachHeight.transform.position.y, arm.pivot.transform.position.z)) <= 0.2)
        {
            raiseArm = true;
        }

    }
    private void RaiseArm(BossArm arm)
    {
        timer2 += arm.armDropSpeed * Time.deltaTime;
        timer2 = Mathf.Clamp01(timer2);
        arm.pivot.transform.position = Vector3.Lerp(arm.pivot.transform.position, new Vector3(arm.pivot.transform.position.x, arm.startPoint.transform.position.y, arm.pivot.transform.position.z), timer2);

        if (Vector3.Distance(arm.pivot.transform.position, new Vector3(arm.pivot.transform.position.x, arm.startPoint.transform.position.y, arm.pivot.transform.position.z)) <= 0.2)
        {
           moveArm = true;
        }
    }
    private void MoveArm(BossArm arm)
    {
        arm.coll.isTrigger = false;
        timer3 += arm.armDropSpeed * Time.deltaTime;
        timer3 = Mathf.Clamp01(timer3);
        arm.transform.position = Vector3.Lerp(arm.transform.position, arm.startPoint.transform.position, timer3);

        if (Vector3.Distance(arm.transform.position, arm.startPoint.transform.position) <= 0.2)
        {
            arm.canTakeDamage = true;
            anim.enabled = true;
            arm.isAttacking = false;            
            SetArmEndAnimation(arm.isRight);
            StartOtherHandAttack();
        }
    }
    public void StartOtherHandAttack()
    {
        if (currentAttackisRight)
        {
            StartLeftAttack();
            currentAttackisRight = false;

        }
        else
        {
            StartRightAttack();
            currentAttackisRight=true;
        }

        timerFollow = 0;
        timer1 = 0;
        timer2 = 0;
        timer3 = 0;
        moveArm = false;
        raiseArm = false;
        

    }
    public void LeftAttack()
    {
        rightArm.isAttacking = false;
        leftArm.isAttacking = true;
        anim.enabled = false;
    }
    public void StartRightAttack()
    {       
        anim.SetTrigger("StartRightAttack");
    }
    public void StartLeftAttack()
    {
        anim.SetTrigger("StartLeftAttack");
    }
    public void SetArmEndAnimation(bool isRight)
    {
        if(isRight)
        {
            anim.SetTrigger("EndRightAttack");
        }
        else
        {
            anim.SetTrigger("EndLeftAttack");
        }
    }
    public void RightAttack()
    {
        leftArm.isAttacking=false;
        rightArm.isAttacking = true;
        anim.enabled = false;
    }
    public void PlayBossMusic()
    {
        GameManager.Instance.audioManager.PlayMusic(GameManager.Instance.bossFightMusic);
    }
    public void PlayAttacksClip()
    {
        int randomInt = UnityEngine.Random.Range(0, attacksClips.Length);
        AudioClip clipToPlay = attacksClips[randomInt];
        GameManager.Instance.audioManager.PlayAudio(clipToPlay);
    }
    public void PlayVoiceClip()
    {
        int randomInt = UnityEngine.Random.Range(0, normalVoiceClip.Length);
        AudioClip clipToPlay = normalVoiceClip[randomInt];
        GameManager.Instance.audioManager.PlayAudio(clipToPlay);
    }
    public void PlayAttackVoiceClip()
    {
        int randomInt = UnityEngine.Random.Range(0, attackVoiceClip.Length);
        AudioClip clipToPlay = attackVoiceClip[randomInt];
        GameManager.Instance.audioManager.PlayAudio(clipToPlay);
    }
    public void StartIntro()
    {
        if (alive)
        {
            anim.SetTrigger("Intro");
            StartOtherHandAttack();
        }
    }

}
