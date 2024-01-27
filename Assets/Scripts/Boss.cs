using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int hpMax;
    [SerializeField] float attackFollowPlayerTimer;
    [SerializeField] public float DelayAttacks;
    [SerializeField] private BossArm leftArm;
    [SerializeField] private BossArm RightArm;
    private Vector3 playerPos;
    
    private int currentHp;
    private Animator anim;
    private void Awake()
    {
        currentHp = hpMax;
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetTrigger("Intro");
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log(currentHp);
        if(currentHp <= 0) 
        {
            anim.SetTrigger("Death");
            Debug.Log("yeeeee boss morto");


        }
    }

    public void Update() 
    {

    }

    public void StartLeftAttack()
    {
        leftArm.isAttacking = true;
        anim.SetTrigger("StartLeftAttack");
    }
    public void LeftAttack()
    {
        leftArm.coll.isTrigger = true;
        playerPos = GameManager.Instance.player.transform.position;
        leftArm.transform.position = new Vector3(playerPos.x, leftArm.transform.position.y, playerPos.z);
        
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
