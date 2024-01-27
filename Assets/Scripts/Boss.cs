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

    private float journeyLength;
    private float startTime;
    private Vector3 playerPos;
    
    private int currentHp;
    private Animator anim;
    private bool starting;
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
        if(currentHp <= 0) 
        {
            anim.SetTrigger("Death");
            Debug.Log("yeeeee boss morto");


        }
    }

    // Calculate the fraction of the journey completed
    
    public void Update() 
    {
        if(leftArm.isAttacking)
        {
            if(!starting)
            {
                startTime = Time.deltaTime;
                starting = true;
            }
            
            journeyLength = Vector3.Distance(transform.position, new Vector3(playerPos.x, leftArm.transform.position.y, playerPos.z));
            float distCovered = (Time.time - startTime) * leftArm.armSpeed;

            playerPos = GameManager.Instance.transform.position;
            float fracJourney = distCovered / journeyLength;

            leftArm.transform.position = Vector3.Lerp(transform.position, new Vector3(playerPos.x, leftArm.transform.position.y, playerPos.z), fracJourney);
        }
        else if(RightArm.isAttacking)
        {

        }
    }

    public void StartLeftAttack()
    {
        leftArm.isAttacking = true;
        anim.SetTrigger("StartLeftAttack");
    }
    public void LeftAttack()
    {
        //Usare lerp , e moveTo
       
        
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
