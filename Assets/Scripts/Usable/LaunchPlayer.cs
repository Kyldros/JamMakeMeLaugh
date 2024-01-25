using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaunchPlayer : MonoBehaviour
{
    public float force = 10f;
    public bool is45Degree = false;


    void ShootObject(Rigidbody objectToShoot)
    {
        Vector3 direction = Vector3.up;
        if (is45Degree)
            direction = new Vector3(1, 1, 0);

        objectToShoot.AddForce( direction * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            ShootObject(other.GetComponent<Rigidbody>());
        }
            

    }

}
