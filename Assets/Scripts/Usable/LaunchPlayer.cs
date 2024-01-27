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

    Animator anim;
    public AnimatorController degree45Cont;
    public AnimatorController degree90Cont;

    void Start()
    {
        anim = GetComponent<Animator>();
        if(is45Degree)
            anim.runtimeAnimatorController = degree45Cont;
        else
            anim.runtimeAnimatorController = degree90Cont;
    }

    void ShootObject(Rigidbody objectToShoot)
    {
        Vector3 direction = Vector3.up;
        if (is45Degree)
        {
            if(otherDirection)
                direction = new Vector3(1, 1, 0);
            else
                direction = new Vector3(-1, 1, 0);
            
        } 

        objectToShoot.AddForce( direction * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (is45Degree)
            {
                if (player.isDashing)
                {
                    ShootObject(player.rb);
                }
            }
            else
            {
                if (player.isRagdoll)
                {
                    ShootObject(player.rb);
                }
            }
        }
    }

}
