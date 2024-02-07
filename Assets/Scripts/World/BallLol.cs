using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLol : MonoBehaviour
{
    public Rigidbody rb;
    public float forceAmount = 10;

    private void Start()
    {
        //rb.AddForce(Vector3.forward * forceAmount, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * forceAmount);
    }
}

   