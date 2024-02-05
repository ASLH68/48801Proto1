using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing : OnSliced
{
    [SerializeField] GameObject hammer;
    [SerializeField] float impulse = 1;
    public override void SlicedAction()
    {
        // Calls the parent sliced action
        base.SlicedAction();

        // object specific functionality
        Debug.Log("derived sliced");

        hammer.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, impulse), ForceMode.Impulse);
    }
}