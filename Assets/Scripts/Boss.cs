using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int hpMax;
    [SerializeField] float attackFollowPlayerTimer;
    [SerializeField] public float DelayAttacks;
    [SerializeField] private BossArm leftArm;
    [SerializeField] private BossArm RightArm;
    [SerializeField] private float attackFollowDuration;
   

    private Vector3 playerPos;

    private int currentHp;
    private Animator anim;

    private float timerAttack;
    private float timerFollow;
    private void Awake()
    {
        currentHp = hpMax;
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetTrigger("Intro");
        anim.SetTrigger("StartLeftAttack");
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log(currentHp);
        if (currentHp <= 0)
        {
            anim.SetTrigger("Death");
            Debug.Log("yeeeee boss morto");


        }
    }


    public void Update()
    {
        if (leftArm.isAttacking)
        {
            
            if (timerFollow >= attackFollowDuration)
            {
                DropArm(leftArm);
            }
            else
            {
                ArmAttack(leftArm);
                timerFollow += Time.deltaTime;
            }
        }
        else if (RightArm.isAttacking)
        {

        }
    }

    private void ArmAttack(BossArm arm)
    {
        arm.coll.isTrigger = false;
        timerAttack += arm.armMoveSpeed * Time.deltaTime;
        timerAttack = Mathf.Clamp01(timerAttack);
        playerPos = GameManager.Instance.player.transform.position;
        arm.transform.position = Vector3.Lerp(arm.transform.position, new Vector3(playerPos.x, arm.transform.position.y, playerPos.z), timerAttack);
    }

    private void DropArm(BossArm arm)
    {
        arm.coll.isTrigger = true;
        timerAttack += arm.armDropSpeed * Time.deltaTime;
        timerAttack = Mathf.Clamp01(timerAttack);
        playerPos = GameManager.Instance.player.transform.position;
        arm.transform.position = Vector3.Lerp(arm.transform.position, new Vector3(arm.transform.position.x, arm.dropReachHeight.transform.position.y, playerPos.z), timerAttack);
        if (Vector3.Distance(arm.transform.position, new Vector3(arm.transform.position.x, arm.dropReachHeight.transform.position.y, playerPos.z)) <= 0.2)
        {
            arm.isAttacking = false;
        }
        
    }
    public void LeftAttack()
    {
        leftArm.isAttacking = true;
    }
    public void StartRightAttack()
    {
        RightArm.isAttacking = true;
        anim.SetTrigger("StartRightAttack");
    }
    public void RightAttack()
    {

    }


}
