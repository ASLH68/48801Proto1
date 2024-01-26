using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoll : OnSliced
{
    [SerializeField] GameObject ball;
    [SerializeField] float impulse = 1;
    public override void SlicedAction()
    {
        // Calls the parent sliced action
        base.SlicedAction();

        // object specific functionality
        Debug.Log("derived sliced");

        ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, impulse), ForceMode.Impulse);
    }
}