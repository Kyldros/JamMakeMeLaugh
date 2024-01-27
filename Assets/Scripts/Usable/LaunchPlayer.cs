using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class LaunchPlayer : MonoBehaviour
{
    public float force = 10f;
    public bool is45Degree = false;
    public bool otherDirection = false;
    public float disableInputDuration = 2f;
    
    Animator anim;
    public AnimatorController degree45Cont;
    public AnimatorController degree90Cont;
    public AnimatorController degree45Loop;
    public AnimatorController degree90loop;
    public Animator loopAnimator;
    public GameObject shotAnimationObject;
    public GameObject visual;
    bool isDisabled = false;

    Player player;
    float startTime;
    void Start()
    {
        anim = GetComponent<Animator>();
        if (is45Degree)
        {
            loopAnimator.runtimeAnimatorController = degree45Loop;
            anim.runtimeAnimatorController = degree45Cont;
        }
        else
        {
            loopAnimator.runtimeAnimatorController = degree90loop;
            anim.runtimeAnimatorController = degree90Cont;
        }
    }

    private void Update()
    {
        if (isDisabled)
        {
            if(Time.time - startTime > disableInputDuration)
            {
                GameManager.Instance.EnablePlayerInput();
                isDisabled = false;
                player.isShooted = false;
            }
        }
    }

    private void StartShoot()
    {
        anim.SetTrigger("L");
        GameManager.Instance.DisablePlayerInput();
        player.transform.parent = shotAnimationObject.transform;
        startTime = Time.time;
        isDisabled = true;
        player.isDashing = false;
        player.SetRagdoll(true);
        player.isShooted = true;
    }

    public void ShootObject()
    {
        player.transform.parent = null;
        Vector3 direction = Vector3.up;
        if (is45Degree)
        {
            if(otherDirection)
                direction = new Vector3(1, 1, 0);
            else
                direction = new Vector3(-1, 1, 0);
            
        } 

        player.rb.AddForce(direction * force, ForceMode.Impulse);
        visual.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
            player = other.GetComponent<Player>();

        if (player != null)
        {
            if (is45Degree)
            {
                if (player.isDashing)
                {
                    StartShoot();
                }
            }
            else
            {
                if (player.isRagdoll)
                {
                    StartShoot();
                }
            }
        }
    }

}
