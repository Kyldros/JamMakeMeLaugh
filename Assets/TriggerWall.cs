using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWall : MonoBehaviour
{
    GameManager gameManager;
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
    }
    private void Start()
    {
        //gameManager.wallList.Add(this.gameObject);
    }

}
