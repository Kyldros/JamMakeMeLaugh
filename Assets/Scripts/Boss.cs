using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int hpMax;
    private int currentHp;
    private void Awake()
    {
        currentHp = hpMax;
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log(currentHp);
        if(currentHp <= 0) 
        {
            Debug.Log("yeeeee boss morto");
        }
    }

   
}
