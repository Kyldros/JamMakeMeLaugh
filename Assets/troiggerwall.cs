using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troiggerwall : MonoBehaviour
{
    
    public GameObject bossWall;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {            
            bossWall.gameObject.active = false;
        }

    }
    }
    
