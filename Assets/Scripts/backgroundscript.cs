using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundscript : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        float newOffset = GameManager.Instance.player.moveDirection.x;
        newOffset *= -1;
        //material.SetTextureOffset()
    }

}
