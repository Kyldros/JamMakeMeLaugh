using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossWall;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            boss.transform.localPosition = new Vector3(boss.transform.position.x, boss.transform.position.y, 7.23f);
            boss.GetComponentInChildren<Boss>().StartIntro();
            bossWall.gameObject.active = false;
        }
    }
}
