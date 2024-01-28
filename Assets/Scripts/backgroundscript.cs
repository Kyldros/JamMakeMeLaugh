using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundscript : MonoBehaviour
{
    public GameObject pivot;
    public Material material;
    public float velocityMultiplyier = 0.5f;
    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        transform.position = pivot.transform.position;
        float newOffset = GameManager.Instance.player.moveDirection.x;
        newOffset = newOffset * 0.1f * velocityMultiplyier;
        if(newOffset!=0)
            material.mainTextureOffset = new Vector2(material.mainTextureOffset.x + newOffset, 0);
    }

}
